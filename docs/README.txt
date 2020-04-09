Twometer VR Documentation
=========================

Protocol Controller<->Service:
  ControllerInfoPacket:
    [int16 length][byte controllerId][byte buttonPressCount][byte[] buttonPresses][float yaw][float pitch][float roll]


Protocol Service<->Driver:
  DriverPacket:
    [int16 length][byte controllerStateCount][ControllerState[] controllerStates][byte buttonPressCount][ButtonPress[] buttonPresses]
  
  ControllerState:
    [byte ctrlId][float posX][float posY][float posZ][float rotX][float rotY][float rotZ]

  ButtonPress:
    [byte ctrlId][byte buttonId]

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
  0: Blue  left hand
  1: Red   right hand