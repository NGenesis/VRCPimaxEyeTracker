# VRCPimaxEyeTracker
VRChat plugin supports avatar eye tracking using the Droolon Pi1 eye tracker for Pimax headsets.

# Installation
> **:warning: Ensure that aSeeVR Runtime (typically installed via PiTool) is installed and running, and that your Droolon Pi1 is correctly installed and calibrated before using the plugin.**

1. Download and install [MelonLoader](https://melonwiki.xyz/) (requires 0.3.0 or higher).
1. Download [VRCPimaxEyeTracker](https://github.com/NGenesis/VRCPimaxEyeTracker/releases) and extract the contents of the archive to any location.
1. Copy `PimaxEyeTrackerNative.dll` and `aSeeVRClient.dll` to your VRChat installation folder, in the same location where `VRChat.exe` is located.
1. Copy `VRCPimaxEyeTracker.dll`to your Mods folder.

# Avatar Creation

## Expression / Animator Parameters
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
| EyesX | Left / Right | Float | -1.0 ~ 1.0 | Returns -1.0 when the user's left or right eye is looking to the left, 0.0 when looking forward and 1.0 when looking to the right. |
| EyesY | Left / Right | Float | -1.0 ~ 1.0 | Returns -1.0 when the user's left or right eye is looking down, 0.0 when looking forward and 1.0 when looking up. |

The recommended setup that will be used in this guide uses the following parameters:

- UseEyeTracker
- LeftEyeBlink
- RightEyeBlink
- LeftEyeX
- RightEyeX
- EyesY

## Blend Shapes
| Name | Preview |
| --- | --- |
| Right Eye Blink | ![Right Eye Blink](docs/blendshapes/Right%20Eye%20Blink.png) |
| Left Eye Blink | ![Left Eye Blink](docs/blendshapes/Left%20Eye%20Blink.png) |
| Eyes Look Forward | ![Eyes Look Forward](docs/blendshapes/Eyes%20Look%20Forward.png) |
| Right Eye Look Right Up | ![Right Eye Look Right Up](docs/blendshapes/Right%20Eye%20Look%20Right%20Up.png) |
| Right Eye Look Up | ![Right Eye Look Up](docs/blendshapes/Right%20Eye%20Look%20Up.png) |
| Right Eye Look Left Up | ![Right Eye Look Left Up](docs/blendshapes/Right%20Eye%20Look%20Left%20Up.png) |
| Right Eye Look Right | ![Right Eye Look Right](docs/blendshapes/Right%20Eye%20Look%20Right.png) |
| Right Eye Look Left | ![Right Eye Look Left](docs/blendshapes/Right%20Eye%20Look%20Left.png) |
| Right Eye Look Right Down | ![Right Eye Look Right Down](docs/blendshapes/Right%20Eye%20Look%20Right%20Down.png) |
| Right Eye Look Down | ![Right Eye Look Down](docs/blendshapes/Right%20Eye%20Look%20Down.png) |
| Right Eye Look Left Down | ![Right Eye Look Left Down](docs/blendshapes/Right%20Eye%20Look%20Left%20Down.png) |
| Left Eye Look Right Up | ![Left Eye Look Right Up](docs/blendshapes/Left%20Eye%20Look%20Right%20Up.png) |
| Left Eye Look Up | ![Left Eye Look Up](docs/blendshapes/Left%20Eye%20Look%20Up.png) |
| Left Eye Look Left Up | ![Left Eye Look Left Up](docs/blendshapes/Left%20Eye%20Look%20Left%20Up.png) |
| Left Eye Look Right | ![Left Eye Look Right](docs/blendshapes/Left%20Eye%20Look%20Right.png) |
| Left Eye Look Left | ![Left Eye Look Left](docs/blendshapes/Left%20Eye%20Look%20Left.png) |
| Left Eye Look Right Down | ![Left Eye Look Right Down](docs/blendshapes/Left%20Eye%20Look%20Right%20Down.png) |
| Left Eye Look Down | ![Left Eye Look Down](docs/blendshapes/Left%20Eye%20Look%20Down.png) |
| Left Eye Look Left Down | ![Left Eye Look Left Down](docs/blendshapes/Left%20Eye%20Look%20Left%20Down.png) |

## Avatar Masks

Create avatar masks `Left Eye` and `Right Eye` using each eye's respective eye bones that will be assigned to the layers below.

## Avatar Constraints

On the left and right eye bones of the avatar:
- Add a `Rotation Constraint` component
- Uncheck `Is Active`
- Set `Weight` to `1`
- Add a dummy transform to the `Head` bone that matches the respective bone to `Source`

These constraints will be used to reset the eye bone transforms while hardware eye tracking is active.

## Layers

### Use Eye Tracker

![Animator](docs/layers/layer_useeyetracker.png)

#### Layer Settings
| Property | Value |
| --- | --- |
| Weight | 1 |
| Mask | None |
| Blending | Override |

#### States

- **Eye Tracker Disabled**
  - Set `Motion` to `Disable Eye Tracker`
  - Add Behaviours:
    - VRCAnimatorTrackingControl - Set `Eyes & Eyelids` to `Tracking`
    - VRCAnimatorLayerControl - Set `Playable` to `FX`, set `Layer` to `2`, and set `Goal Weight` to `0`
    - VRCAnimatorLayerControl - Set `Playable` to `FX`, set `Layer` to `3`, and set `Goal Weight` to `0`
    - VRCAnimatorLayerControl - Set `Playable` to `FX`, set `Layer` to `4`, and set `Goal Weight` to `0`
    - VRCAnimatorLayerControl - Set `Playable` to `FX`, set `Layer` to `5`, and set `Goal Weight` to `0`

- **Eye Tracker Enabled**
  - Set `Motion` to `Enable Eye Tracker`
  - Add Behaviours:
    - VRCAnimatorTrackingControl - Set `Eyes & Eyelids` to `Animation`
    - VRCAnimatorLayerControl - Set `Playable` to `FX`, and set `Layer` to `2`, and set `Goal Weight` to `1`
    - VRCAnimatorLayerControl - Set `Playable` to `FX`, and set `Layer` to `3`, and set `Goal Weight` to `1`
    - VRCAnimatorLayerControl - Set `Playable` to `FX`, and set `Layer` to `4`, and set `Goal Weight` to `1`
    - VRCAnimatorLayerControl - Set `Playable` to `FX`, and set `Layer` to `5`, and set `Goal Weight` to `1`

#### Transitions

| Source State | Destination State | Conditions |
| --- | --- | --- |
| Any State | Eye Tracker Disabled | UseEyeTracker: false |
| Any State | Eye Tracker Enabled | UseEyeTracker: true |

### Left Eye Blink

![Animator](docs/layers/layer_lefteyeblink.png)

#### Layer Settings

| Property | Value |
| --- | --- |
| Weight | 1 |
| Mask | Left Eye |
| Blending | Override |

#### States

- **Eye Tracker Disabled**
  - Set `Motion` to `None`
- **Left Eye Open**
  - Set `Motion` to `Left Eye Open`
- **Left Eye Close**
  - Set `Motion` to `Left Eye Close`

#### Transitions

| Source State | Destination State | Conditions |
| --- | --- | --- |
| Any State | Eye Tracker Disabled | UseEyeTracker: false |
| Any State | Left Eye Open | LeftEyeBlink: false, UseEyeTracker: true |
| Any State | Left Eye Closed | LeftEyeBlink: true, UseEyeTracker: true |

### Right Eye Blink

![Animator](docs/layers/layer_righteyeblink.png)

#### Layer Settings

| Property | Value |
| --- | --- |
| Weight | 1 |
| Mask | Right Eye |
| Blending | Override |

#### States

- **Eye Tracker Disabled**
  - Set `Motion` to `None`
- **Right Eye Open**
  - Set `Motion` to `Right Eye Open`
- **Right Eye Close**
  - Set `Motion` to `Right Eye Close`

#### Transitions

| Source State | Destination State | Conditions |
| --- | --- | --- |
| Any State | Eye Tracker Disabled | UseEyeTracker: false |
| Any State | Right Eye Open | RightEyeBlink: false, UseEyeTracker: true |
| Any State | Right Eye Closed | RightEyeBlink: true, UseEyeTracker: true |

### Left Eye Movement

![Animator](docs/layers/layer_lefteyemovement.png)

#### Layer Settings

| Property | Value |
| --- | --- |
| Weight | 1 |
| Mask | Left Eye |
| Blending | Override |

- **Eye Tracker Disabled**
  - Set `Motion` to `None`

- **Left Eye Movement**
  - Create as Blend Tree
  - Set `Blend Type` to `2D Freeform Directional`
  - Set `Parameters` to `LeftEyeX` and `EyesY`
  - Add Motions:

| Motion | Pos X | Pos Y |
| --- | --- | --- |
| Left Eye Look Up | 0 | 1 |
| Left Eye Look Down | 0 | -1 |
| Left Eye Look Left | -1 | 0 |
| Left Eye Look Right | 1 | 0 |
| Left Eye Look Forward | 0 | 0 |
| Left Eye Look Left Up | -1 | 1 |
| Left Eye Look Right Up | 1 | 1 |
| Left Eye Look Left Down | -1 | -1 |
| Left Eye Look Right Down | 1 | -1 |

![Blend Tree](docs/layers/layerstate_lefteyemovement.png)

#### Transitions

| Source State | Destination State | Conditions |
| --- | --- | --- |
| Any State | Eye Tracker Disabled | UseEyeTracker: false |
| Any State | Left Eye Movement | UseEyeTracker: true |

### Right Eye Movement

![Animator](docs/layers/layer_righteyemovement.png)

#### Layer Settings

| Property | Value |
| --- | --- |
| Weight | 1 |
| Mask | Right Eye |
| Blending | Override |

- **Eye Tracker Disabled**
  - Set `Motion` to `None`

- **Right Eye Movement**
  - Create as Blend Tree
  - Set `Blend Type` to `2D Freeform Directional`
  - Set `Parameters` to `RightEyeX` and `EyesY`
  - Add Motions:

| Motion | Pos X | Pos Y |
| --- | --- | --- |
| Right Eye Look Up | 0 | 1 |
| Right Eye Look Down | 0 | -1 |
| Right Eye Look Left | -1 | 0 |
| Right Eye Look Right | 1 | 0 |
| Right Eye Look Forward | 0 | 0 |
| Right Eye Look Left Up | -1 | 1 |
| Right Eye Look Right Up | 1 | 1 |
| Right Eye Look Left Down | -1 | -1 |
| Right Eye Look Right Down | 1 | -1 |

![Blend Tree](docs/layers/layerstate_righteyemovement.png)

#### Transitions

| Source State | Destination State | Conditions |
| --- | --- | --- |
| Any State | Eye Tracker Disabled | UseEyeTracker: false |
| Any State | Right Eye Movement | UseEyeTracker: true |

## Animations

### Disable Eye Tracker

| Property | Value |
| --- | --- |
| VRC Avatar Descriptor.Custom Eye Look Settings.eyelid Type | 2 |
| Eye_L : Rotation Constraint.Active | false |
| Eye_R : Rotation Constraint.Active | false |

### Enable Eye Tracker

| Property | Value |
| --- | --- |
| Body: Skinned Mesh Renderer.Blend Shape.Eyes Blink | 0 |
| Eye_L : Rotation Constraint.Active | true |
| Eye_R : Rotation Constraint.Active | true |

### Left Eye Close

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Blink | 100 |

### Left Eye Look Down

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 0 |

### Left Eye Look Forward

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 0 |

### Left Eye Look Left

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 0 |

### Left Eye Look Left Down

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 0 |

### Left Eye Look Left Up

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 0 |

### Left Eye Look Right

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 0 |

### Left Eye Look Right Down

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 0 |

### Left Eye Look Right Up

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 0 |

### Left Eye Look Up

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Look Up | 100 |

### Left Eye Open

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Left Eye Blink | 0 |

### Right Eye Close

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Blink | 100 |

### Left Eye Look Down

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 0 |

### Left Eye Look Forward

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 0 |

### Left Eye Look Left

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 0 |

### Left Eye Look Left Down

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 0 |

### Left Eye Look Left Up

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 0 |

### Left Eye Look Right

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 0 |

### Left Eye Look Right Down

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 0 |

### Left Eye Look Right Up

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 100 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 0 |

### Left Eye Look Up

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Eyes Look Forward | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Left Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Down | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Right Up | 0 |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Look Up | 100 |

### Right Eye Open

| Property | Value |
| --- | --- |
| Body : Skinned Mesh Renderer.Blend Shape.Right Eye Blink | 0 |
