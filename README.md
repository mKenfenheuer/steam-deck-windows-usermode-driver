# Usermode controller driver for the Steam Deck build in controller

This work-in-progress driver maps the Steam deck's builtin controller to a virtual ViGEm XBox 360 Controller.
The layout is similar to an XBox One Elite controller (Grip Buttons R4,L4,R5,L5 map to A,B,X,Y).

## How to try? 

1. Install ViGEm Bus driver from [here](https://github.com/ViGEm/ViGEmBus/releases).
1. Download the latest release from the releases section or build it yourself (see below).
2. Make sure Steam is closed. Steam doesnt like us messing with their controller.
3. Run the .exe file inside the zip, or install the whole solution via the .msi
4. Configure your settings in the app_config.conf (blacklist or whitelist processes)
6. Have fun

## Known Bugs

* Steam still produces controller inputs even if we are hooking the hid device. Therefore with the driver active you will get doubled inputs in BigPicture mode and steam games if the driver is not disabled by config.
* The driver sometimes fails to grab the serial number / fails to initialize. Make sure steam is closed when launching the driver. If that doesnt help, reboot.

Please report any bugs you encounter using the Issues section. Don't forget to provide a meaningful description such as What does not work? How should it work? What did you do? Makes it easier for all of us.

## How to build?

### Prerequesites:
* Visual Studio 2019+
    * Windows SDK
    * C++ Support

### Building

1. Open the `.sln` file using Visual Studio
2. Select the `SteamDeckControllerWindowsDriver` Project
3. Click Build

### Known Errors

* Build may fail due to incompatible architectures. Make sure x64 is selected for all projects inside the solution.
* Build may fail due to incompatible debug runtimes Make sure they are all the same for all projects.

