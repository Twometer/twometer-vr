#define CONTROLLER_PORT 12742
#define DISCOVERY_PORT  12743

#include "WiFiConfig.h"
#include "Discovery.h"

#define CONTROLLER_RED
// #define CONTROLLER_BLUE

#ifdef CONTROLLER_RED
  #define CONTROLLER_ID   0
#elif defined(CONTROLLER_BLUE)
  #define CONTROLLER_ID   1
#endif

Discovery discovery;

void setup() {
  Serial.begin(38400);    // Ah yes, debug
  Serial.println("Twometer VR Firmware v2.0");

  WiFi.persistent(true);  // Save those credentials
  WiFi.mode(WIFI_STA);    // Station mode for ESP-firmware glitch prevention
  WiFi.begin(WIFI_SSID, WIFI_PASS);
  pinMode(14, INPUT_PULLUP);

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
  // TODO connect
  Serial.println("Connection established");
}

void loop() {

}
