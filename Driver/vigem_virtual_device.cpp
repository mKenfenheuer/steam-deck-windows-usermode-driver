#define LOG_TAG "vigem_vdev"
#include "logging.h"
#include "vigem_virtual_device.h"

PVIGEM_CLIENT _vigem_client;
PVIGEM_TARGET _vigem_target;

void vigem_close_device()
{
	vigem_target_remove(_vigem_client, _vigem_target);
	vigem_target_free(_vigem_target);
}

bool vigem_open_device()
{
	_vigem_client = vigem_alloc();

	if (_vigem_client == nullptr)
	{
		LOG_ERROR("Could not connect to ViGEm, lack of memory!");
		return false;
	}

	auto retval = vigem_connect(_vigem_client);

	if (!VIGEM_SUCCESS(retval))
	{
		LOG_ERROR("ViGEm Bus connection failed with error code: 0x%2x", retval);
		return false;
	}

	_vigem_target = vigem_target_x360_alloc();

	auto pir = vigem_target_add(_vigem_client, _vigem_target);

	if (!VIGEM_SUCCESS(pir))
	{
		LOG_ERROR("Target plugin failed with error code: 0x%2x", pir);
		return false;
	}

	return true;
}

bool vigem_update_device(XUSB_REPORT report)
{
	auto pir = vigem_target_x360_update(_vigem_client, _vigem_target, report);
	if (!VIGEM_SUCCESS(pir))
	{
		LOG_ERROR("Could not update virtual controller status. Error 0x%2x", pir);
		return false;
	}
	return true;
}

bool vigem_reset_device()
{
	XUSB_REPORT report = XUSB_REPORT();
	auto pir = vigem_target_x360_update(_vigem_client, _vigem_target, report);
	if (!VIGEM_SUCCESS(pir))
	{
		LOG_ERROR("Could not update virtual controller status. Error 0x%2x", pir);
		return false;
	}
	return true;
}