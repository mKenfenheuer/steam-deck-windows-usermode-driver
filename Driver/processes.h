#pragma once
#include <vector>
#include <string>
using namespace std;
// Gets a list of running processes
// 
// vector<string>* list: the pointer to the list of processes to fill
// 
void sys_get_process_list(vector<string>* list);