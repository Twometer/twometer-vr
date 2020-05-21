// #define CONTROLLER_RED
#define CONTROLLER_BLUE

#define PACKET_RATE  75
#define PACKET_DELAY (1000.0 / PACKET_RATE)

#include <ESP8266WiFi.h>

#include "util/Constants.h"
#include "net/WiFiManager.h"
#include "net/Discovery.h"
#include "util/Timer.h"
#include "util/Storage.h"
#include "pose/IPoseSource.h"
// #include "pose/SwPoseSource.h"
#include "pose/DmpPoseSource.h"
#include "util/Button.h"
#include "net/Packet.h"

Button trigger(TRIGGER_PIN);

WiFiManager wifi;
Discovery discovery;
IPAddress serverIp;
Timer packetTimer;
WiFiUDP udp;

// Here, select software or DMP pose source
IPoseSource *imu = new DmpPoseSource();

void setup() {
  Serial.begin(38400);    // Ah yes, debug
  Serial.println("Twometer VR Firmware v2.1");

  pinMode(TRIGGER_PIN, INPUT_PULLUP); // Configure our pin

  Serial.println("Connecting to WiFi..."); // Some WiFi
  wifi.connect();
  Serial.println("Connected");

  Serial.println("Discovering server..."); // And a server
  serverIp = discovery.discover();
  Serial.print("Discovered server at ");
  Serial.println(serverIp);

  if (!udp.begin(CONTROLLER_PORT)) { // Configure UDP client
    Serial.println("Can't initialize UDP client");
    ESP.restart();
  }
  Packet::SendStatusPacket(&udp, serverIp, STATUS_CONNECTED);

  Serial.println("Initializing IMU...");
  imu->begin();
  Serial.println("Initialized");

  Storage storage;

  // If the storage has data and the trigger is pressed at startup
  if (storage.hasData() && digitalRead(TRIGGER_PIN) == LOW) {
    delay(2500); // And the trigger is held for 2.5s
    if (digitalRead(TRIGGER_PIN) == LOW) {
      Packet::SendStatusPacket(&udp, serverIp, STATUS_RESET);
      delay(500);
      storage.clear(); // We do a factory reset
      ESP.restart();
    }
  }

  // Calibration steps
  if (imu->requiresCalibration()) {
    Serial.println("Calibrating...");
    Packet::SendStatusPacket(&udp, serverIp, STATUS_ENTER_CALIB);
    delay(4000);
    imu->calibrateAccelGyro();

    Packet::SendStatusPacket(&udp, serverIp, STATUS_CALIB_MAG);
    delay(4000);
    imu->calibrateMagnetometer();

    Packet::SendStatusPacket(&udp, serverIp, STATUS_EXIT_CALIB);
    delay(4000);
    Serial.println("Calibration completed");
  }

  Serial.println("Calculating offsets...");
  Packet::SendStatusPacket(&udp, serverIp, STATUS_CALC_OFFSETS);
  imu->calculateOffsets();

  Serial.println("Controller is ready!");
  Packet::SendStatusPacket(&udp, serverIp, STATUS_READY);
}

void loop() {
  if (imu->update() && packetTimer.elapsed(PACKET_DELAY)) {
    sendPackets();
    packetTimer.reset();
  }
}

void sendPackets() {
  if (trigger.isPressed()) {
    byte buttons[] = { BUTTON_A };
    Packet::SendDataPacket(&udp, serverIp, 1, buttons, imu->getQx(), imu->getQy(), imu->getQz(), imu->getQw());
  } else {
    Packet::SendDataPacket(&udp, serverIp, 0, NULL, imu->getQx(), imu->getQy(), imu->getQz(), imu->getQw());
  }
}
