#define _CRT_SECURE_NO_WARNINGS
#define LOG_TAG "sys_processes"

#include "logging.h"
#include <Windows.h>
#include <tlhelp32.h>
#include <string>
#include <vector>
using namespace std;


void sys_get_process_list(vector<string>* list) {
	HANDLE hProcessSnap;
	PROCESSENTRY32 pe32;
	hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

	list->clear();

	if (hProcessSnap == INVALID_HANDLE_VALUE) {
		LOG_ERROR("Could not open handle for process list.");
	}
	else {
		pe32.dwSize = sizeof(PROCESSENTRY32);
		if (Process32First(hProcessSnap, &pe32)) { // Gets first running process
			char processName[261];
			sprintf(processName, "%ws", pe32.szExeFile);
			list->push_back(string(processName));
			// loop through all running processes
			while (Process32Next(hProcessSnap, &pe32)) {
				sprintf(processName, "%ws", pe32.szExeFile);
				list->push_back(string(processName));
			}
		}
		// clean the snapshot object
		CloseHandle(hProcessSnap);
	}
}