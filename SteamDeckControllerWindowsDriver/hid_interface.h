#pragma once
// Opens the hid device associated with the specified vendor id and product id.
// 
// vendor_id:	unsigned int
// product_id:	unsigned int
//
// Returns bool - indicates if the device was successfully opened
bool hid_open_device(unsigned int vendor_id, unsigned int product_id);
// Performs an hid request to the opened device
// 
// vendor_id:	unsigned int
// product_id:	unsigned int
//
// Returns uint8_t* - response of the device
uint8_t* hid_request(uint8_t* data, int32_t length);
// Performs an hid interrupt read with timeout
// 
// uint8_t* data: the data buffer
// size_t length: the amount of bytes to read max
// int timeout: the timeout in milliseconds
//
// Returns int - the amount of data read
int hid_read_timeout(uint8_t* data, size_t length, int timeout);

// Closes the hid device
void hid_close_device();
