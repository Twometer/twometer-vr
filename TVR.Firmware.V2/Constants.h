// General constants such as the controller type, pin maps, buttons and ports

#ifdef CONTROLLER_RED
  #define CONTROLLER_ID   0
#elif defined(CONTROLLER_BLUE)
  #define CONTROLLER_ID   1
#endif

#define TRIGGER_PIN 14

#define CONTROLLER_PORT 12742
#define DISCOVERY_PORT  12743

#define BUTTON_NONE   0
#define BUTTON_A      1
#define BUTTON_B      2
