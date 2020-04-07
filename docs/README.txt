Twometer VR Documentation
=========================

Protocol Controller<->Service:
  ControllerInfoPacket:
    [int32 buttonPressCount][int32[] buttonPresses][float rotX][float rotZ]


Protocol Service<->Driver:
  DriverPacket:
    [int32 length][int32 controllerStateCount][ControllerState[] controllerStates][int32 buttonPressCount][ButtonPress[] buttonPresses]
  
  ControllerState:
    [int32 ctrlId][float posX][float posY][float posZ][float rotX][float rotY]

  ButtonPress:
    [int32 ctrlId][int32 buttonId]

Button IDs:

- 0 None
- 1 Up
- 2 Down
- 3 Left
- 4 Right
- 5 Center

How many of those buttons will be actually used is yet to be determined.
This is just the table with reserved IDs