# VRCPimaxEyeTracker
VRChat plugin supports avatar eye tracking using the Droolon Pi1 eye tracker for Pimax headsets.

## Installation
> **:warning: Ensure that aSeeVR Runtime (typically installed via PiTool) is installed and running, and that your Droolon Pi1 is correctly installed and calibrated before using the plugin.**

> **:warning: If VRChat hangs when launching with the plugin installed, it may be nessecary to run VRChat as Administrator.  To do this, right click `VRChat.exe`, navigate to `Properties` > `Compatibility` tab and check `Run this program as an administrator`.**

1. Download and install [MelonLoader](https://melonwiki.xyz/) (requires 0.3.0 or higher).
1. Download [VRCPimaxEyeTracker](https://github.com/NGenesis/VRCPimaxEyeTracker/releases) and extract the contents of the archive to any location.
1. Copy `PimaxEyeTrackerNative.dll` and `aSeeVRClient.dll` to your VRChat installation folder, in the same location where `VRChat.exe` is located.
1. Copy `VRCPimaxEyeTracker.dll`to your Mods folder.

## Avatar Setup

### Expression Parameters
| Name | Eye | Type | Value | Description |
| --- | --- | --- | --- | --- |
| UseEyeTracker | | Bool | True / False | When set to true by the user via the quick menu or an animator, hardware eye tracking will be enabled. |
| LeftEyeBlink | Left | Bool | True / False | Returns true when the user's left eye is closed. |
| RightEyeBlink | Right | Bool | True / False | Returns true when the user's right eye is closed. |
| LeftEyeLid | Left | Float | 0.0 ~ 1.0 | Returns 0.0 when the user's left eye is fully closed and 1.0 when fully open. |
| RightEyeLid | Right | Float | 0.0 ~ 1.0 | Returns 0.0 when the user's right eye is fully closed and 1.0 when fully open. |
| LeftEyeX | Left | Float | -1.0 ~ 1.0 | Returns -1.0 when the user's left eye is looking to the left, 0.0 when looking forward and 1.0 when looking to the right. |
| RightEyeX | Right | Float | -1.0 ~ 1.0 | Returns -1.0 when the user's right eye is looking to the left, 0.0 when looking forward and 1.0 when looking to the right. |
| LeftEyeY | Left | Float | -1.0 ~ 1.0 | Returns -1.0 when the user's left eye is looking down, 0.0 when looking forward and 1.0 when looking up. |
| RightEyeY | Right | Float | -1.0 ~ 1.0 | Returns -1.0 when the user's right eye is looking down, 0.0 when looking forward and 1.0 when looking up. |
| EyesY | Left / Right | Float | -1.0 ~ 1.0 | Returns -1.0 when the user's left or right eye is looking down, 0.0 when looking forward and 1.0 when looking up. |
