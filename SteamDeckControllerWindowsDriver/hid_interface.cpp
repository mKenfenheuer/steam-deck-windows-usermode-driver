#define _CRT_SECURE_NO_WARNINGS
#define LOG_TAG "hid_interface"

#include "logging.h"
#include "hidapi.h"
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#define MAX_STR 255
#define BUFFER_MAX 255

hid_device* _hid_dev_handle = NULL;
int res;

bool hid_open_device(unsigned int vendor_id, unsigned int product_id)
{
	// Initialize the hidapi library
	res = hid_init();

	// Open the device using the VID, PID,
	// and optionally the Serial number.

	struct hid_device_info* devs, * cur_dev;
	const char* path_to_open = NULL;

	devs = hid_enumerate(vendor_id, product_id);
	cur_dev = devs;

	while (cur_dev && _hid_dev_handle == NULL) {
		if (cur_dev->vendor_id == vendor_id &&
			cur_dev->product_id == product_id) {
			LOG_DEBUG("Trying device at path %s", cur_dev->path);
			_hid_dev_handle = hid_open_path(cur_dev->path);
		}
		cur_dev = cur_dev->next;
	}

	// If the device handle is still null, we could not open any of the matching devices. 
	// This is bad.
	if (_hid_dev_handle == NULL) {
		return false;
	}
	return true;
}

void hid_close_device()
{
	if (_hid_dev_handle != NULL)
		hid_close(_hid_dev_handle);
}

uint8_t* hid_request(uint8_t* data, int32_t _length)
{
	if (_hid_dev_handle == NULL)
	{
		LOG_ERROR("Device is not open, cannot perform hid request!");
	}

	uint16_t length;
	bool use_same_buffer = false;
	unsigned char buffer[BUFFER_MAX + 1];
	uint8_t* out_buffer = NULL;
	int err;

	if (_length < 0) {
		use_same_buffer = true;
		length = (uint16_t)(-_length);
		out_buffer = data;
	}
	else {
		length = (uint16_t)_length;
		out_buffer = (uint8_t*)malloc(length);
		if (out_buffer == NULL) {
			LOG_ERROR("sccd_input_hid_request: OOM Error");
			return NULL;
		}
	}

	if (length > BUFFER_MAX) {
		LOG_ERROR("called with length larger than supported. Changing BUFFER_MAX will fix this issue");
		return NULL;
	}

	buffer[0] = 0;
	memcpy(&buffer[1], data, length);

	err = hid_send_feature_report(_hid_dev_handle, buffer, length + 1);
	if (err < 0) {
		wcstombs((char*)buffer, hid_error(_hid_dev_handle), BUFFER_MAX);
		LOG_ERROR("Could not send hid feature report: %s", buffer);
		goto hid_request_fail;
	}

	err = hid_get_feature_report(_hid_dev_handle, buffer, length + 1);
	if (err < 0) {
		wcstombs((char*)buffer, hid_error(_hid_dev_handle), BUFFER_MAX);
		LOG_ERROR("Could not get hid feature report: %s", buffer);
		goto hid_request_fail;
	}

	memcpy(out_buffer, &buffer[1], length);
	return out_buffer;

hid_request_fail:
	free(out_buffer);

	return NULL;
}

int hid_read_timeout(uint8_t* data, size_t length, int timeout)
{
	return hid_read_timeout(_hid_dev_handle, data, length, timeout);
}