Twometer VR Documentation
=========================

Protocol Controller<->Service:
  ControllerInfoPacket:
    [int16 length][byte controllerId][byte buttonPressCount][byte[] buttonPresses][float yaw][float pitch][float roll]


Protocol Service<->Driver:
  DriverPacket:
    [int16 length][byte controllerStateCount][ControllerState[] controllerStates]
  
  ControllerState:
    [byte ctrlId][float posX][float posY][float posZ][float rotX][float rotY][float rotZ][byte buttonCount][ButtonState[] states]

  ButtonState:
    [byte btnId][bool pressed]

Coordinate system:
  X: To the right
  Y: To the top
  Z: Away from the screen

  Rotation X: Pitch
  Rotation Y: Yaw
  Rotation Z: Roll

Button IDs:
  0: None
  1: A
  2: B

How many of those buttons will be actually used is yet to be determined.
This is just the table with reserved IDs

Controller IDs:
  0: Red   left hand
  1: Blue  right hand


How to use with a DK2
=====================
This works as of April 2020!

1. Download the latest official Oculus app, install WITH THE HEADSET DISCONNECTED!
   If you install with the headset connected it will fail.

2. Start the Oculus app, and connect the headset. It will ask you to configure it, skip that
   because it will not work. If you now look through the headset, you should see the Oculus Home VR screen.

3. In the Oculus app, under Settings->General enable unknown sources if it is not.

4. Install SteamVR and the TVR driver using the installer script

To make it work with Beat Saber (and other legacy-controller games):
********************************************************************

5. Start SteamVR, and go to the controller mapping config for Beat Saber

6. Make Button A of left and right controller the Left and Right Trigger, respecitvely.

7. In the 2nd Tab with legacy settings, set pretend to be controller to "Oculus Touch"