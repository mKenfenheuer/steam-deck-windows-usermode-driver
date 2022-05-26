#pragma once
//Checks if a certain process is running
// 
// const char* name: the name of the process
// 
// Returns:
// bool - indicates whether the process is running
bool sys_check_process_running(const char* name);