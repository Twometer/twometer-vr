#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#include "WiFiConfig.h"

#define CONTROLLER_PORT 12742
#define DISCOVERY_PORT  12743

WiFiUDP udp;
byte discoverySequence[4] = { 0x79, 0x65, 0x65, 0x74 };
IPAddress broadcastIp(255, 255, 255, 255);

WiFiClient tcp;

String serverIp;
char udp_incoming[15];

void setup() {
  Serial.begin(9600);     // Ah yes, debug
  Serial.println("Twometer VR Firmware v1.0");
  
  WiFi.persistent(true);  // Save those credentials
  WiFi.mode(WIFI_STA);    // Station mode for ESP-firmware glitch prevention
  WiFi.begin(WIFI_SSID, WIFI_PASS);

  Serial.println("Connecting to WiFi...");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
  }
  Serial.println("Connected");

  Serial.println("Discovering server...");
  while (!discovery()) {
    delay(500);
  }
  Serial.print("Discovered server at ");
  Serial.println(serverIp);

  Serial.println("Connecting to server...");
  while (!tcp.connect(serverIp, CONTROLLER_PORT)) {
    delay(500);
  }
  Serial.println("Connection established");
}

void loop() {
  // Read accelerometer
  // Read buttons
  // Send it to the server
}

bool discovery() {
  udp.beginPacket(broadcastIp, DISCOVERY_PORT);
  udp.write(discoverySequence, 4);
  udp.endPacket();

  int packetSize = udp.parsePacket();
  if (!packetSize)
    return false;

  int len = udp.read(udp_incoming, packetSize);
  if (len <= 0)
    return false;

  serverIp = String(udp_incoming);
  return true;
}
