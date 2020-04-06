Twometer VR Documentation
=========================

Protocol Controller<->Service:
  MetaFrame:
    [int32[] buttonPresses][float rotX][float rotZ]


Protocol Service<->Driver:
  DataPacket transmission:    
    [int32 length][int32 controllerStateCount][ControllerState[] controllerStates][int32 buttonPressCount][ButtonPress[] buttonPresses]
  
  ControllerState:
    [int32 ctrlId][float posX][float posY][float posZ][float rotX][float rotY]

  ButtonPress:
    [int32 ctrlId][int32 buttonId]