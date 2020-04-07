Twometer VR Documentation
=========================

Protocol Controller<->Service:
  ControllerInfoPacket:
    [int16 length][byte controllerId][byte buttonPressCount][byte[] buttonPresses][float rotX][float rotZ]


Protocol Service<->Driver:
  DriverPacket:
    [int16 length][byte controllerStateCount][ControllerState[] controllerStates][byte buttonPressCount][ButtonPress[] buttonPresses]
  
  ControllerState:
    [byte ctrlId][float posX][float posY][float posZ][float rotX][float rotY]

  ButtonPress:
    [byte ctrlId][byte buttonId]

Button IDs:

- 0 None
- 1 Up
- 2 Down
- 3 Left
- 4 Right
- 5 Center

How many of those buttons will be actually used is yet to be determined.
This is just the table with reserved IDs