#define CONTROLLER_RED
// #define CONTROLLER_BLUE

#include <ESP8266WiFi.h>

#include "Constants.h"
#include "WiFiConfig.h"
#include "Discovery.h"
#include "Button.h"
#include "Packet.h"

Button trigger(TRIGGER_PIN);
Discovery discovery;
WiFiClient tcp;

void setup() {
  Serial.begin(38400);    // Ah yes, debug
  Serial.println("Twometer VR Firmware v2.0");

  pinMode(TRIGGER_PIN, INPUT_PULLUP); // Configure our pin

  WiFi.persistent(true);  // Save those credentials
  WiFi.mode(WIFI_STA);    // Station mode for ESP-firmware glitch prevention
  WiFi.begin(WIFI_SSID, WIFI_PASS); // Connect

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

  if (!tcp.connected()) {
    // TODO Reconnect here
  }
}
