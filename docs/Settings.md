# Settings
Settings allows you to make configuration changes within the application.

## Start with Windows
Enabling this setting will start SWICD when Windows starts.  Use this setting if you plan to launch a game using the gamepad (i.e. using Xbox Controller Bar or another game launcher with controller support), or if you do not want to start the SWICD application manual.  

## Operation Mode
Select the operation mode that you would like SWICD to operate in.  

There are three options: 
* Blacklist
* Whitelist
* Combined

### Blacklist
Use Blacklist mode to prevent SWICD from emulating input if a process with the same executable name is running. The mode must be set to blacklist or combined, if not this setting is ignored.

When Blacklist or Combined mode is used, blacklist an application by clicking on the **Add** button at the bottom of the Blacklisted Processes list.  Navigate to the application you wish to blacklist, select it, and then click **Open** to add it to the blacklisted processes list.

### Whitelist 
Use Blacklist mode to prevent SWICD from emulating input unless a process with the same executable name is running. The mode must be set to whitelist or combined, if not this setting is ignored.

When Whitelist or Combined mode is used, whitelist an application by clicking on the **Add** button at the bottom of the Whitelisted Processes list.  Navigate to the application you wish to whitelist, select it, and then click **Open** to add it to the whitelisted processes list.

### Combined
Combined mode uses both the whitelist and blacklist.

## HID Hiding
Work in Progress (HIDHide will prevent Steam from grabbing input of the internal controller. This can lead to doubled inputs.)
