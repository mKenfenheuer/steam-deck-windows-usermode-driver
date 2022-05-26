#define _CRT_SECURE_NO_WARNINGS
#define LOG_TAG "sys_processes"

#include "logging.h"
#include <Windows.h>
#include <tlhelp32.h>


bool sys_check_process_running(const char* name) {
	HANDLE hProcessSnap;
	PROCESSENTRY32 pe32;
	hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

	if (hProcessSnap == INVALID_HANDLE_VALUE) {
		return false;
	}
	else {
		pe32.dwSize = sizeof(PROCESSENTRY32); if (Process32First(hProcessSnap, &pe32)) { // Gets first running process
			char processName[256];
			sprintf(processName, "%ws", pe32.szExeFile);
			if (strncmp(processName, name, sizeof(processName)) == 0)
			{
				return true;
			}
			// loop through all running processes looking for process
			while (Process32Next(hProcessSnap, &pe32)) {
				sprintf(processName, "%ws", pe32.szExeFile);
				if (strncmp(processName, name, sizeof(processName)) == 0)
				{
					return true;
				}
			}
		}
		// clean the snapshot object
		CloseHandle(hProcessSnap);
	}
	LOG_DEBUG("There is no process running with executable name: %s", name);
	return false;
}