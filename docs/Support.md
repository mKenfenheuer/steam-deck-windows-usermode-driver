This page will show you how to get support, open an Issue if needed, known issues, and contains an FAQs section for commonly run into issues and asked questions.

## How to get support
Mainly support is provided through the [issues section](https://github.com/mKenfenheuer/steam-deck-windows-usermode-driver/issues). Please adhere the issue templates as precisely as possible (see below).

If you'd like to get in touch with Maximilian or other users directly you are welcome to join the discord server: https://discord.gg/pxgNz53H

## Issues
Issues are current (open) and past (closed) issues with SWICD, these can be bugs or feature requests. Looking through closed tickets can sometimes help you find a resolution to a problem that you are having.

- [Open Issues](https://github.com/mKenfenheuer/steam-deck-windows-usermode-driver/issues?q=is%3Aopen+is%3Aissue) 
- [Closed Issues](https://github.com/mKenfenheuer/steam-deck-windows-usermode-driver/issues?q=is%3Aissue+is%3Aclosed)

### Opening an Issue
If you don't find the issue you are having in either the Open or Closed Issues lists, please open an Issue using the SWICD Issue Template below.  Click on the [![New Issue](/docs/images/New%20Issue.png)](https://github.com/mKenfenheuer/steam-deck-windows-usermode-driver/issues/new/choose) button on the [Open Issues](https://github.com/mKenfenheuer/steam-deck-windows-usermode-driver/issues?q=is%3Aopen+is%3Aissue) page to open a new issue.  You will be presented with two possible categories of Issue to open.

**Available Categories**
- [BUG] - to be used when running into unexpected behavior or problems preventing the use of the software or a feature within the software
- [FEATURE] - to be used with requesting that a feature be added

#### Issue Templates
Please use the issue templates when submitting an issue.  Replace the text under each section in the template with the requested information.

## Known Bugs
- Steam still produces controller inputs even if we are hooking the hid device. Therefore with the driver active you will get doubled inputs in BigPicture mode and steam games if the driver is not disabled by config.
- The driver sometimes fails to grab the serial number / fails to initialize. Make sure steam is closed when launching the driver. If that doesnt help, reboot.

For a list of more known bugs please visit the [Open Issues](https://github.com/mKenfenheuer/steam-deck-windows-usermode-driver/issues?q=is%3Aopen+is%3Aissue) page

## Frequently Asked Questions / Common Issues
Q. I am getting double inputs, or unexepected inputs

A. Try disabling Lizard Mode.  This can cause double inputs.  Lizard mode will also cause the right trigger (left-click) to scroll downwards on some Windows applications.  You can fix this by mapping **R2** to **None** while Lizard Mode is enabled.

Q. The controller is not working in any games

A. Make sure that you either a) have SWICD in Blacklist only mode or b) have the executable for the game you are trying to play in the whitelist when using whitelist or combined mode.

Q. I am getting an error in the Driver Log stating "Could not open neptune controller: System.DllNotFoundException: Unable to load DLL 'hidapi.dll': The specified module could not be found...."

A. You are missing the Visual C++ Redistributable that contains the hidapi.dll file.  Install from here: [Microsoft Visual C++ Redistributable Package](https://aka.ms/vs/17/release/vc_redist.x64.exe)