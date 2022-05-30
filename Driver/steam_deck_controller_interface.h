#pragma once


// Gets the steam deck controller serial number
//
// Returns char* -	returns the controllers serial number null terminated.
bool sdc_get_serial(char* buffer);
// Enables or disabled the steam deck controllers lizard mode
// (mouse and keyboard emulation)
// 
// bool enabled: enable or disable the lizard mode
//
// Returns bool - indicates success
bool sdc_set_lizard_mode(bool enabled);
bool sdc_set_haptic(uint8_t amount);
// Enables or disabled the steam deck controllers lizard mode
// (mouse and keyboard emulation)
// 
// uint16_t idle_timeout: idle timeout in seconds
// bool gyro_enabled: enable or disable the gyro
//
// Returns bool - indicates success
bool sdc_configure(uint16_t idle_timeout, bool gyro_enabled);
// Clears the controller mappings
//
// Returns bool - indicates success
bool sdc_clear_mappings();