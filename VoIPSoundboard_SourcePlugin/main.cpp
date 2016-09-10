#include "stdafx.h"
#define PLUGINMESSAGE_PLAYSOUND 0x00
#define PLUGINMESSAGE_STOPPLAYING 0x01
#define PLUGINMESSAGE_STARTRECORDING 0x02
#define PLUGINMESSAGE_STOPRECORDING 0x03

#if _DEBUG
	#define DEBUGMSG(message) printf(message)
	#define DEBUGMSG1(message, data1) printf(message, data1)
	#define DEBUGMSG2(message, data1, data2) printf(message, data1, data2)
#else
	#define DEBUGMSG(message) 
	#define DEBUGMSG1(message, data1) 
	#define DEBUGMSG2(message, data1, data2) 
#endif

typedef void (__stdcall *ClientCMDProc)(const char*, bool);
typedef struct sockaddr_in sockaddr_in;

DWORD GetModuleSize(HMODULE hModule)
{
	if (hModule == 0)
	{
		return 0;
	}
	MODULEINFO modinfo = { 0 };
	GetModuleInformation(GetCurrentProcess(), hModule, &modinfo, sizeof(MODULEINFO));
	return modinfo.SizeOfImage;
}
DWORD FindClientCMD(DWORD startPos, DWORD lookLength)
{
	bool found = false;
	char* mask = "11111    11    111    11";
	char* pattern = "\x55\x8B\xEC\x8B\x0D\x00\x00\x00\x00\x81\xF9\x00\x00\x00\x00\x75\x0C\xA1\x00\x00\x00\x00\x35\x90";
	int patternLength = strlen(mask);
	DWORD endPos = (startPos + lookLength) - patternLength;
	DEBUGMSG2("Looking for pattern from 0x%08x to 0x%08x\n", startPos, endPos + patternLength);
	for (DWORD currMemoryPos = startPos; currMemoryPos < endPos; currMemoryPos++)
	{
		found = true;
		//Foreach byte from currMemoryPos to (currMemoryPos + patternLength)
		for (int currPatternOffset = 0; currPatternOffset < patternLength; currPatternOffset++)
		{
			if (mask[currPatternOffset] == '1' && pattern[currPatternOffset] != *(char*)(currMemoryPos + currPatternOffset))
			{
				found = false;
				break;
			}
		}
		if (found)
		{
			DEBUGMSG2("Found ClientCMD method @ 0x%08x (Offset: +0x%08x)\n", currMemoryPos, currMemoryPos - startPos);
			return currMemoryPos;
		}
	}
	return NULL;
}
void PluginService()
{
	ClientCMDProc ClientCMD;
	DWORD engineDLL = (DWORD)GetModuleHandleA("engine.dll");
	DWORD engineDLLSize = GetModuleSize((HMODULE)engineDLL);
	DEBUGMSG1("EngineDLL Module @ 0x%08x\n", engineDLL);
	DEBUGMSG1("EngineDLLSize: %i\n", engineDLLSize);
	ClientCMD = (ClientCMDProc)FindClientCMD(engineDLL, engineDLLSize);
	if (ClientCMD != NULL)
	{
		SOCKET serverSocket;
		serverSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
		if (serverSocket != INVALID_SOCKET)
		{
			sockaddr_in serverAddress;
			DEBUGMSG("Socket created.\n");
			serverAddress.sin_family = AF_INET;
			serverAddress.sin_addr.s_addr = INADDR_ANY;
			serverAddress.sin_port = htons(60873);
			if (bind(serverSocket, (sockaddr *)&serverAddress, sizeof(serverAddress)) != SOCKET_ERROR)
			{
				SOCKET clientSocket;
				sockaddr_in clientAddress;
				int clientAddressSize = sizeof(sockaddr_in);
				DEBUGMSG("Bind done.\n");
				listen(serverSocket, 3);
				while (TRUE)
				{
					DEBUGMSG("Listening for new client. . .\n");
					clientSocket = accept(serverSocket, (sockaddr*)&clientAddress, &clientAddressSize);
					DEBUGMSG("Client connected.\n");
					if (clientSocket != INVALID_SOCKET)
					{
						char buffer[1];
						int receivedBytes = 1;
						int bufferLength = sizeof(buffer);
						while (receivedBytes > 0)
						{
							receivedBytes = recv(clientSocket, buffer, bufferLength, 0);
							DEBUGMSG1("Received: %i\n", buffer[0]);
							switch (buffer[0])
							{
								case PLUGINMESSAGE_PLAYSOUND:
									DEBUGMSG("Playing sound\n");
									ClientCMD("voice_inputfromfile 1; +voicerecord; voice_loopback 1", false);
								break;
								case PLUGINMESSAGE_STOPPLAYING:
									DEBUGMSG("Stopping sound\n");
									ClientCMD("-voicerecord; voice_inputfromfile 0; voice_loopback 0", false);
								break;
								case PLUGINMESSAGE_STARTRECORDING:
									//...
								break;
								case PLUGINMESSAGE_STOPRECORDING:
									//...
								break;
								default:
									DEBUGMSG("Unknown packet\n");
								break;
							}
						}
						if (receivedBytes != 0)
						{
							DEBUGMSG1("recv failed with error: %d\n", WSAGetLastError());
						}
						closesocket(clientSocket);
						WSACleanup();
					}
					else
					{
						DEBUGMSG1("Accept failed with error code : %d\n", WSAGetLastError());
					}
					Sleep(1250);
				}
			}
			else
			{
				DEBUGMSG1("Bind failed with error code : %d\n", WSAGetLastError());
			}
		}
		else
		{
			DEBUGMSG1("Could not create socket : %d\n", WSAGetLastError());
		}
	}
	else
	{
		DEBUGMSG("Could not find the ClientCMD method with the given signature\n");
	}
}
BOOL APIENTRY DllMain(HMODULE hModule, DWORD ulReason, LPVOID lpReserved)
{
	if (ulReason == DLL_PROCESS_ATTACH)
	{
		#if _DEBUG
			AllocConsole();
			freopen("CON", "w", stdout);
		#endif
		CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)PluginService, hModule, 0, NULL);
	}
	return TRUE;
}
