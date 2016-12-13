#pragma once
#include "stdafx.h"

typedef void(__stdcall *ClientCMDProc)(const char* command);

DWORD GetModuleSize(HMODULE hModule);
DWORD FindPattern(DWORD startPos, DWORD lookLength, const char *pattern, const char *mask);
bool DetourMethod(void *source, void *destination, int length);
void PluginService(ClientCMDProc ClientCMD);
