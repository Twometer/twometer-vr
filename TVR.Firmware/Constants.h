// General constants such as the controller type, pin maps, buttons and ports

#ifndef TVR_CONSTANTS
#define TVR_CONSTANTS

  #ifdef CONTROLLER_RED
    #define CONTROLLER_ID   0
  #elif defined(CONTROLLER_BLUE)
    #define CONTROLLER_ID   1
  #endif

  #define TRIGGER_PIN 14

  #define CONTROLLER_PORT 12742
  #define DISCOVERY_PORT  12743

  // Button enum
  #define BUTTON_NONE   0
  #define BUTTON_A      1
  #define BUTTON_B      2

  // Status enum
  #define STATUS_CONNECTED    0x00
  #define STATUS_ENTER_CALIB  0x01
  #define STATUS_CALIB_MAG    0x02
  #define STATUS_EXIT_CALIB   0x03
  #define STATUS_CALC_OFFSETS 0x04
  #define STATUS_READY        0x05
  #define STATUS_RESET        0x06

#endif //TVR_CONSTANTS
