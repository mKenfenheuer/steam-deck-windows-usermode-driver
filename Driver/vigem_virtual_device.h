#pragma once
#define WIN32_LEAN_AND_MEAN

#ifdef WIN32_LEAN_AND_MEAN
#include <windows.h>
#endif

// ViGEm Library
#include "ViGEm/Client.h"
#include "ViGEm/Common.h"
//
// Link against SetupAPI of ViGEm
//
#pragma comment(lib, "setupapi.lib")

// Closes the ViGEm emulated device
void vigem_close_device();
// Opens a new ViGEm emulated device
//
// Returns bool - indicates if the device was successfully opened
bool vigem_open_device();
// Updates the status of the device
//
// Returns bool - indicates if the device was successfully updated
bool vigem_update_device(XUSB_REPORT report);
// Resets the status of the device
//
// Returns bool - indicates if the device was successfully reset
bool vigem_reset_device();
bool vigem_register_notifications(PFN_VIGEM_X360_NOTIFICATION notification);
