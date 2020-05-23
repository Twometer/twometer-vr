#define CONTROLLER_RED
// #define CONTROLLER_BLUE

#include <ESP8266WiFi.h>

#include "util/Constants.h"
#include "util/Timer.h"
#include "util/Storage.h"
#include "util/Button.h"
#include "pose/IPoseSource.h"
#include "pose/SwPoseSource.h"
#include "pose/DmpPoseSource.h"
#include "net/WiFiManager.h"
#include "net/UdpClient.h"
#include "net/Discovery.h"
#include "net/Packet.h"

WiFiManager wifi;
Discovery discovery;
UdpClient udpClient;

Button trigger(PIN_TRIG);
Timer packetTimer(PACKET_RATE);

IPoseSource *imu = new DmpPoseSource();   // Select DMP or Madgwick

void setup() {
  // Ah yes, debug
  Serial.begin(38400);
  Serial.println("Twometer VR Firmware v2.1");

  // Hardware setup
  trigger.init();

  // Some WiFi
  Serial.println("Connecting to WiFi...");
  wifi.connect();
  Serial.println("Connected");

  // Find the server
  Serial.println("Discovering server...");
  IPAddress serverIp = discovery.discover();
  Serial.print("Discovered server at: "); Serial.println(serverIp);

  // Configure UDP client
  udpClient.begin(serverIp, CONTROLLER_PORT);
  Packet::sendStatusPacket(udpClient, STATUS_CONNECTED);

  // Connect to the inertial measurement unit
  Serial.println("Initializing IMU...");
  imu->begin();
  Serial.println("Initialized");

  // Do we need a factory reset?
  checkFactoryReset();

  // Do we need to calibrate?
  checkCalibration();

  // And done
  Serial.println("Controller is ready!");
  Packet::sendStatusPacket(udpClient, STATUS_READY);
}

void loop() {
  if (imu->update() && packetTimer.elapsed()) {
    sendPackets();
    packetTimer.reset();
  }
}

void sendPackets() {
  if (trigger.isPressed()) {
    byte buttons[] = { BUTTON_A };
    Packet::sendDataPacket(udpClient, 1, buttons, imu->getQx(), imu->getQy(), imu->getQz(), imu->getQw());
  } else {
    Packet::sendDataPacket(udpClient, 0, NULL, imu->getQx(), imu->getQy(), imu->getQz(), imu->getQw());
  }
}

void checkFactoryReset() {
  Storage storage;
  if (!storage.hasData()) return;

  // If the trigger is held down for 2.5 seconds
  if (digitalRead(PIN_TRIG) != LOW) return;
  delay(2500);
  if (digitalRead(PIN_TRIG) != LOW) return;

  // Perform a reset
  Packet::sendStatusPacket(udpClient, STATUS_RESET);
  delay(500);
  storage.clear();
  ESP.restart();
}

void checkCalibration() {
  if (imu->requiresCalibration()) {
    Serial.println("Entering calibration mode...");

    Packet::sendStatusPacket(udpClient, STATUS_ENTER_CALIB);
    delay(4000);
    imu->calibrateAccelGyro();

    Packet::sendStatusPacket(udpClient, STATUS_CALIB_MAG);
    delay(4000);
    imu->calibrateMagnetometer();

    Packet::sendStatusPacket(udpClient, STATUS_EXIT_CALIB);
    delay(4000);

    Serial.println("Exiting calibration mode...");
  }
}
