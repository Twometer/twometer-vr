#include "RotationSensor.h"
#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#include "WiFiConfig.h"
#include "Button.h"
#include "ButtonId.h"

#define CONTROLLER_PORT 12742
#define DISCOVERY_PORT  12743


// IMPORTANT: Define whether you are flashing the red (left) or blue (right) controller

// #define CONTROLLER_RED
#define CONTROLLER_BLUE

#ifdef CONTROLLER_RED
#define CONTROLLER_ID   0
#elif defined(CONTROLLER_BLUE)
#define CONTROLLER_ID   1
#endif

Button trigger(14); // Main button

WiFiUDP udp;
byte discoverySequence[4] = { 0x79, 0x65, 0x65, 0x74 };
IPAddress broadcastIp;

WiFiClient tcp;

String serverIp;
char udp_incoming[15];

RotationSensor sensor;

unsigned long last_update;

void setup() {
  Serial.begin(38400);     // Ah yes, debug
  Serial.println("Twometer VR Firmware v1.0");

  WiFi.persistent(true);  // Save those credentials
  WiFi.mode(WIFI_STA);    // Station mode for ESP-firmware glitch prevention
  WiFi.begin(WIFI_SSID, WIFI_PASS);

  Serial.println("Connecting to WiFi...");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
  }
  Serial.println("Connected");

  broadcastIp = ~uint32_t(WiFi.subnetMask()) | uint32_t(WiFi.gatewayIP());
  Serial.print("Broadcast IP: ");
  Serial.println(broadcastIp);

  Serial.print("Local IP: ");
  Serial.println(WiFi.localIP());

  Serial.println("Discovering server...");
  udp.begin(DISCOVERY_PORT);
  while (!discovery()) {
    delay(500);
  }
  Serial.print("Discovered server at ");
  Serial.println(serverIp);

  Serial.println("Initializing MPU...");
  sensor.begin();
  Serial.println("Initialized");

  Serial.println("Connecting to server...");
  tcp.setNoDelay(true);
  while (!tcp.connect(serverIp, CONTROLLER_PORT)) {
    delay(500);
  }
  Serial.println("Connection established");
  pinMode(14, INPUT_PULLUP);
}

void loop() {
  sensor.update();

  if (millis() - last_update > 80) {
    if (trigger.isHeld()) {
      byte btn[] = {BUTTON_A};
      sendPacket(1, btn, sensor.getYaw(), sensor.getPitch(), sensor.getRoll());
    } else sendPacket(0, NULL, sensor.getYaw(), sensor.getPitch(), sensor.getRoll());
    last_update = millis();
  }
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

void sendPacket(byte numButtonPresses, byte* buttonPresses, float yaw, float pitch, float roll) {

  int16_t packetLen = 2 + 1 + 1 + (numButtonPresses) + 4 * 3;
  byte data[packetLen];
  int offset = 0;

  cpy(data, offset, int16_t(packetLen - 2));
  cpy(data, offset, byte(CONTROLLER_ID));
  cpy(data, offset, numButtonPresses);
  for (int i = 0; i < numButtonPresses; i++) {
    cpy(data, offset, buttonPresses[i]);
  }
  cpy(data, offset, yaw);
  cpy(data, offset, pitch);
  cpy(data, offset, roll);

  tcp.write(data, packetLen);
  tcp.flush();
}

template<typename T>
void cpy(byte* dst, int &offset, T data) {
  memcpy(dst + offset, &data, sizeof(data));
  offset += sizeof(data);
}
