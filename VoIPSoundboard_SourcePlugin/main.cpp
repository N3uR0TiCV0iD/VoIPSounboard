#include "stdafx.h"
#define PLUGINMESSAGE_PLAYSOUND 0x00
#define PLUGINMESSAGE_STOPPLAYING 0x01
#define PLUGINMESSAGE_STARTRECORDING 0x02
#define PLUGINMESSAGE_STOPRECORDING 0x03

#define CLIENTCMD_PATTERN "\x55\x8B\xEC\x8B\x0D\x00\x00\x00\x00\x81\xF9\x00\x00\x00\x00\x75\x0C\xA1\x00\x00\x00\x00\x35\x00\x00\x00\x00\xEB\x05\x8B\x01\xFF\x50\x00\x85\xC0\x79"
#define CLIENTCMD_MASK "11111    11    111    1    111111 111"
#define ENGINEUPDATEMSG_PATTERN "\x8B\x5D\x0C\x03\x5D\x14\x56\x8B\xF1\x57\x83\xFB\x0C" //Starts @ mov ebx, [ebp+0C]
#define ENGINEUPDATEMSG_MASK "1111111111111"
#define ENGINEUPDATEMSG_REPLACINGBYTES 6
#define SHOWHIDECHATBOX_PATTERN "\x33\xC0\x84\xD2\x51\x0F\x95\xC0" //Starts @ xor eax, eax
#define SHOWHIDECHATBOX_MASK "11111111"
#define SHOWHIDECHATBOX_REPLACINGBYTES 5

#if _DEBUG
	#define DEBUGMSG(message, ...) printf(message, __VA_ARGS__)
#else
	#define DEBUGMSG(message, ...) 
#endif

typedef struct sockaddr_in sockaddr_in;

int chatBoxOpen;
SOCKET clientSocket;
bool clientConnected;
DWORD engineUpdateMsgReturnAddress;
DWORD showHideChatBoxReturnAddress;
_declspec(naked) void EngineUpdateMessage_Detour(const char *data1, int data1Length, const char *data2, int data2Length) //Not sure if that is what the function is for... But hey it suits the purpose :D
{
	__asm
	{
		pushad; //Push all general purpose registers
		pushfd; //Push all CPU flags
	}
	//DEBUGMSG("Detour got called! | data1[%i] = <%s> | data2[%i] = <%s>\n", data1Length, data1, data2Length, data2);
	if (clientConnected && data2Length > 1 && ((data1Length == 10 && strcmp(data1, "say_team \"") == 0) ||
		                                       (data1Length == 5 && strcmp(data1, "say \"") == 0)) )
	{
		char lengthBuffer[4] { //In Little-Endian
			data2Length & 0xFF,
			(data2Length >> 8) & 0xFF,
			(data2Length >> 16) & 0xFF,
			data2Length >> 24,
		};
		DEBUGMSG("Looks like I should TTS: <%s>\n", data2);
		send(clientSocket, lengthBuffer, sizeof(int), 0);
		send(clientSocket, data2, data2Length, 0);
	}
	__asm
	{
		popfd; //Pop all CPU flags
		popad; //Pop all general purpose registers
		mov ebx, [ebp + 0x0C];		//]
		add ebx, [ebp + 0x14];		//]--- Original instructions before detour
		jmp engineUpdateMsgReturnAddress;  //Jump back to the rest of the normal EngineUpdateMessage
	}
}
_declspec(naked) void ShowHideChatBox_Detour()
{
	__asm
	{
		mov chatBoxOpen, edx; //Let's store the value of edx (which happens to contain whether the chatbox is open)
	}
	#if _DEBUG
		__asm
		{
			pushad; //Push all general purpose registers
			pushfd; //Push all general purpose registers
		}
		DEBUGMSG("Detour got called! | chatBoxOpen = %i\n", chatBoxOpen);
		__asm
		{
			popfd; //Pop all CPU flags
			popad; //Pop all general purpose registers
		}
	#endif
	__asm
	{
		xor eax, eax;		//]
		test dl, dl;		//|--- Original instructions before detour
		push ecx;			//]
		jmp showHideChatBoxReturnAddress; //Jump back to the rest of the normal ShowHideChatBox
	}
}
void InitPlugin()
{
	ClientCMDProc ClientCMD;
	DWORD engineDLL = (DWORD)GetModuleHandleA("engine.dll");
	DWORD engineDLLSize = GetModuleSize((HMODULE)engineDLL);
	DEBUGMSG("EngineDLL Module @ 0x%08x\n", engineDLL);
	DEBUGMSG("EngineDLLSize: %i\n", engineDLLSize);
	ClientCMD = (ClientCMDProc)FindPattern(engineDLL, engineDLLSize, CLIENTCMD_PATTERN, CLIENTCMD_MASK);
	if (ClientCMD != NULL)
	{
		DWORD scaleFormDLL = (DWORD)GetModuleHandleA("scaleformui.dll");
		DWORD scaleFormDLLSize = GetModuleSize((HMODULE)scaleFormDLL);
		DWORD engineUpdateMsgHookAddress = FindPattern(scaleFormDLL, scaleFormDLLSize, ENGINEUPDATEMSG_PATTERN, ENGINEUPDATEMSG_MASK);
		DEBUGMSG("Found ClientCMD method @ 0x%08x\n", &ClientCMD);
		DEBUGMSG("scaleFormDLL Module @ 0x%08x\n", scaleFormDLL);
		DEBUGMSG("scaleFormDLLSize: %i\n", scaleFormDLLSize);
		engineUpdateMsgReturnAddress = engineUpdateMsgHookAddress + ENGINEUPDATEMSG_REPLACINGBYTES;
		if (engineUpdateMsgHookAddress != NULL && DetourMethod((void*)engineUpdateMsgHookAddress, &EngineUpdateMessage_Detour, ENGINEUPDATEMSG_REPLACINGBYTES))
		{
			DWORD showHideChatBoxHookAddress = FindPattern(scaleFormDLL, scaleFormDLLSize, SHOWHIDECHATBOX_PATTERN, SHOWHIDECHATBOX_MASK);
			DEBUGMSG("Successfully hooked EngineUpdateMessage @ 0x%08x\n", engineUpdateMsgHookAddress);
			showHideChatBoxReturnAddress = showHideChatBoxHookAddress + SHOWHIDECHATBOX_REPLACINGBYTES;
			if (showHideChatBoxHookAddress != NULL && DetourMethod((void*)showHideChatBoxHookAddress, &ShowHideChatBox_Detour, SHOWHIDECHATBOX_REPLACINGBYTES))
			{
				DEBUGMSG("Successfully hooked ShowHideChatBox @ 0x%08x\n", showHideChatBoxHookAddress);
				PluginService(ClientCMD);
			}
		}
		else
		{
			DEBUGMSG("Could not hook the EngineUpdateMessage method\n");
		}
	}
	else
	{
		DEBUGMSG("Could not find the ClientCMD method with the given signature\n");
	}
}
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
DWORD FindPattern(DWORD startPos, DWORD lookLength, const char *pattern, const char *mask)
{
	bool found = false;
	int patternLength = strlen(mask);
	DWORD endPos = (startPos + lookLength) - patternLength;
	DEBUGMSG("Looking for pattern from 0x%08x to 0x%08x\n", startPos, endPos + patternLength);
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
			return currMemoryPos;
		}
	}
	return NULL;
}
bool DetourMethod(void *source, void *destination, int length)
{
	if (length >= 5)
	{
		DWORD currProtection;
		VirtualProtect(source, length, PAGE_EXECUTE_READWRITE, &currProtection);
		memset(source, 0x90, length);
		DWORD relativeAddress = ((DWORD)destination - (DWORD)source) - 5;
		*(byte*)source = 0xE9;
		*(DWORD*)((DWORD)source + 1) = relativeAddress;
		VirtualProtect(source, length, currProtection, NULL);
		return true;
	}
	else
	{
		return false;
	}
}
void PluginService(ClientCMDProc ClientCMD)
{
	SOCKET serverSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (serverSocket != INVALID_SOCKET)
	{
		sockaddr_in serverAddress;
		DEBUGMSG("Socket created.\n");
		serverAddress.sin_family = AF_INET;
		serverAddress.sin_port = htons(60873);
		serverAddress.sin_addr.s_addr = inet_addr("127.0.0.1");
		if (bind(serverSocket, (sockaddr*)&serverAddress, sizeof(serverAddress)) != SOCKET_ERROR)
		{
			int clientAddressSize = sizeof(sockaddr_in);
			sockaddr_in clientAddress;
			DEBUGMSG("Bind done.\n");
			listen(serverSocket, 3);
			while (TRUE)
			{
				DEBUGMSG("Listening for new client. . .\n");
				clientSocket = accept(serverSocket, (sockaddr*)&clientAddress, &clientAddressSize);
				DEBUGMSG("Client connected.\n");
				if (clientSocket != INVALID_SOCKET)
				{
					char receivedMessage;
					int receivedBytes = 1;
					clientConnected = true;
					while (receivedBytes > 0)
					{
						receivedBytes = recv(clientSocket, &receivedMessage, 1, 0);
						DEBUGMSG("Received: %i\n", receivedMessage);
						switch (receivedMessage)
						{
							case PLUGINMESSAGE_PLAYSOUND:
								if (!chatBoxOpen)
								{
									DEBUGMSG("Playing sound\n");
									ClientCMD("voice_inputfromfile 1; +voicerecord; voice_loopback 1");
								}
							break;
							case PLUGINMESSAGE_STOPPLAYING:
								if (!chatBoxOpen)
								{
									DEBUGMSG("Stopping sound\n");
									ClientCMD("-voicerecord; voice_inputfromfile 0; voice_loopback 0");
								}
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
					clientConnected = false;
					if (receivedBytes != 0)
					{
						DEBUGMSG("recv failed with error: %i\n", WSAGetLastError());
					}
					closesocket(clientSocket);
					WSACleanup();
				}
				else
				{
					DEBUGMSG("Accept failed with error code: %i\n", WSAGetLastError());
				}
				Sleep(1250);
			}
		}
		else
		{
			DEBUGMSG("Bind failed with error code: %i\n", WSAGetLastError());
		}
	}
	else
	{
		DEBUGMSG("Could not create socket: %i\n", WSAGetLastError());
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
		CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)InitPlugin, hModule, 0, NULL);
	}
	return TRUE;
}
