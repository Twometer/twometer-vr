#define CONTROLLER_PORT 12742
#define DISCOVERY_PORT  12743

#include "WiFiConfig.h"
#include "Discovery.h"
#include "ButtonId.h"
#include "Button.h"
#include "Packet.h"

#define CONTROLLER_RED
// #define CONTROLLER_BLUE

#ifdef CONTROLLER_RED
  #define CONTROLLER_ID   0
#elif defined(CONTROLLER_BLUE)
  #define CONTROLLER_ID   1
#endif

#define TRIGGER_PIN 14

Button trigger(TRIGGER_PIN);
Discovery discovery;
WiFiClient tcp;

void setup() {
  Serial.begin(38400);    // Ah yes, debug
  Serial.println("Twometer VR Firmware v2.0");

  WiFi.persistent(true);  // Save those credentials
  WiFi.mode(WIFI_STA);    // Station mode for ESP-firmware glitch prevention
  WiFi.begin(WIFI_SSID, WIFI_PASS); // Connect
  pinMode(TRIGGER_PIN, INPUT_PULLUP); // Configure our pin

  Serial.println("Connecting to WiFi...");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
  }
  Serial.println("Connected.");

  Serial.println("Discovering server...");
  String serverIp = discovery.discover();
  Serial.print("Discovered server at ");
  Serial.println(serverIp);

  // TODO initialize sensors here

  Serial.println("Connecting to server...");
  tcp.setNoDelay(true);
  while (!tcp.connect(serverIp, CONTROLLER_PORT)) {
    delay(500);
  }
  Serial.println("Connection established");
}

void loop() {
  // TODO update sensors
  if (trigger.isPressed()) {

  }
  // TODO send packets:  Packet::Send()
}
