// #define CONTROLLER_RED
#define CONTROLLER_BLUE

#include <ESP8266WiFi.h>

#include "Constants.h"
#include "WiFiConfig.h"
#include "Discovery.h"
#include "IMUController.h"
#include "Button.h"
#include "Packet.h"
#include "Storage.h"

Button trigger(TRIGGER_PIN);
Discovery discovery;

IPAddress serverIp;
WiFiUDP udp;

IMUController imu;

void setup() {
  Serial.begin(38400);    // Ah yes, debug
  Serial.println("Twometer VR Firmware v2.1");

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
  serverIp = discovery.discover();
  Serial.print("Discovered server at ");
  Serial.println(serverIp);
  if (!udp.begin(CONTROLLER_PORT))
    Serial.println("Can't initialize UDP client");

  Packet::SendStatusPacket(&udp, serverIp, STATUS_CONNECTED);

  Serial.println("Initializing MPU...");
  imu.begin();
  Serial.println("Initialized");

  Storage storage;

  // If the storage has data and the trigger is pressed at startup
  if (storage.hasData() && trigger.isPressed()) {
    delay(2500); // And the trigger is held for 2.5s
    if (trigger.isPressed()) {
      storage.clear(); // We do a factory reset
      ESP.restart();
      return;
    }
  }

  if (storage.hasData()) {
    Serial.println("EEPROM has data!");
    storage.loadCalibrationData(imu.getMpu());
  } else {
    Serial.println("Calibrating...");
    Packet::SendStatusPacket(&udp, serverIp, STATUS_ENTER_CALIB);
    imu.calibrateAccelGyro();
    Packet::SendStatusPacket(&udp, serverIp, STATUS_CALIB_MAG);
    delay(4000);
    imu.calibrateMagnetometer();
    Packet::SendStatusPacket(&udp, serverIp, STATUS_EXIT_CALIB);
    storage.storeCalibrationData(imu.getMpu());

    delay(5000);
  }

  Serial.println("Calculating offsets...");
  Packet::SendStatusPacket(&udp, serverIp, STATUS_CALC_OFFSETS);
  imu.calcOffsets();

  Serial.println("Controller is ready!");
  Packet::SendStatusPacket(&udp, serverIp, STATUS_READY);
}

void loop() {
  if (imu.update()) {
    sendPackets();
  }
}

void sendPackets() {
  if (trigger.isPressed()) {
    byte buttons[] = { BUTTON_A };
    Packet::SendDataPacket(&udp, serverIp, 1, buttons, imu.getYaw(), imu.getPitch(), imu.getRoll());
  } else {
    Packet::SendDataPacket(&udp, serverIp, 0, NULL, imu.getYaw(), imu.getPitch(), imu.getRoll());
  }
}
