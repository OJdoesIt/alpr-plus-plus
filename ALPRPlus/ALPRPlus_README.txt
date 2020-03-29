ALPR+ for GTA V
Version 1.0.0
Copyright (C) 2016 Stealth22. All rights reserved.

Any unauthorized modification, reverse engineering, or distribution (including re-uploading of any kind) is strictly prohibited.

-------------------------
Requirements
-------------------------
- You must have version 0.41 (or newer) of the RAGE Plugin Hook installed. You must have a fully legal, non-pirated version of GTA V.
- The version of Stealth.Common.dll included with this package is v1.6.0.1. Older versions will not work; v1.6.0.1 is the same version included with Code 3 Callouts v1.2.0.
- You must have LSPDFR 0.3.1 installed
- ALPR+ WILL CHECK what versions of RPH and Stealth.Common you have installed. It will refuse to run if incorrect versions are installed.

-------------------------
Automatic Installation
-------------------------
- Run "ALPRPlus.msi" to install the plugin
- The installer package requires Administrator rights to run
- The setup package will attempt to detect your Grand Theft Auto installation path using the registry. If the install path is not found, your copy of GTA V has not been properly installed

-------------------------
Manual Installation
-------------------------
- Copy the ALPPlus.dll and ALPPlus.ini files into the Plugins folder of your GTA V directory.
- Copy the Stealth.Common.dll file into your main/root GTA V directory.

-------------------------
Features
-------------------------
- Inspired by the Realistic ANPR plugin by Danielle
- Realistic detection logic which takes various factors into consideration, including vehicle distance, angle, line of sight, camera field of view, etc
- When alerts are triggered, the player only receives information that the ALPR system would know
    - You don't get a blip on your map! The only information you receive is the vehicle model, color, which camera triggered the alert, and the license plate
- Configuration file with customizable camera settings (for advanced users only; default values are highly recommended)
- Eight different types of ALPR alerts, each with it's own potential probability weight
    - Stolen Vehicle
    - Outstanding Warrant
    - Suspended Driver's License
    - Expired Driver's License
    - Vehicle Registration Expired
    - Vehicle Not Registered
    - Vehicle Insurance Expired
    - No Vehicle Insurance
- ALPR system automatically disables itself during traffic stops or pursuits, and re-enables them when the situation is Code 4
- Integration with LSPDFR API
    - NOTE: ALPR+ does change some vehicle and ped records; LSPDFR natively generates a lot of peds with outstanding warrants, or expired/suspended licenses
- Sound notifications when alerts are triggered
- Integration with Traffic Policer API (optional feature)
- Developer API
    - Set (or clear) either a pre-defined or custom flag on a specific vehicle
    - Events that are triggered when an event is either generated, or displayed
    - Get the ALPR flag for a specific vehicle
    - Get a list of ALPR results for the current session

-------------------------
Usage
-------------------------
- Press the toggle keybind (F8 by default) to activate the system