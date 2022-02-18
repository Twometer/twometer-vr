#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <stdint.h>

#include "IO/ButtonInput.h"
#include "IO/PoseInput.h"
#include "Model/TrackerClass.h"
#include "Model/TrackerColor.h"
#include "Net/Discovery.h"
#include "Net/NetDefs.h"
#include "Net/UdpClient.h"
#include "Net/WiFiCredentials.h"
#include "Utils/Constants.h"
#include "Utils/SerialNo.h"

const TrackerClass trackerClass = TrackerClass::Controller;
const TrackerColor trackerColor = TrackerColor::Blue;

ButtonInput buttonInput;
PoseInput poseInput;
UdpClient client;

uint8_t trackerId;

void setupHardware() {
    Logger::info("Setting up hardware...");
    buttonInput.begin();
    poseInput.begin();
}

void setupWifi() {
    Logger::info("Connecting to WiFi " WIFI_SSID "...");
    WiFi.persistent(true);
    WiFi.mode(WIFI_STA);
    WiFi.begin(WIFI_SSID, WIFI_PASS);

    while (WiFi.status() != WL_CONNECTED) {
        delay(100);
    }

    Logger::info("Connected.");
}

void runServerDiscovery() {
    Logger::info("Attempting to discover server...");
    Discovery discovery;
    IPAddress serverIp = discovery.discover();
    client.begin(serverIp, UNICAST_PORT);
    Serial.print("Server IP: ");
    Serial.println(serverIp);
    Logger::info("Successfully found server.");
}

void runServerRegistration() {
    Logger::info("Registering with the server...");
    Packets::sendHello(client, trackerClass, trackerColor, SerialNo::get());
    Packets::receiveHello(client, trackerId);
    Serial.printf("Tracker id: %d\n", trackerId);
    Logger::info("Successfully registered with the server.");
}

void setup() {
    Serial.begin(115200);
    Serial.println("");
    Serial.println(VERSION_STRING);

    setupHardware();
    setupWifi();
    runServerDiscovery();
    runServerRegistration();

    Logger::info("Initialization sequence completed.");
}

void runTick() {
    buttonInput.update();
    poseInput.update();

    if (poseInput.available()) {
        Packets::sendState(client, trackerId, buttonInput.getStates(), poseInput.getPose());
        poseInput.clearAvailable();
    }
}

unsigned long long lastDebug = 0;
void debugTick() {
    if (millis() - lastDebug > 1000) {
        Serial.println("--- calib info ---");
        auto calibData = poseInput.getCalibData();
        Serial.printf("Accel: %d %d %d\n", calibData.accelBias[0], calibData.accelBias[1], calibData.accelBias[2]);
        Serial.printf(" Gyro: %d %d %d\n", calibData.gyroBias[0], calibData.gyroBias[1], calibData.gyroBias[2]);
        Serial.printf("  Mag: %d %d %d\n", calibData.magBias[0], calibData.magBias[1], calibData.magBias[2]);
        lastDebug = millis();
    }
}

void loop() {
    uint64_t tickStart = micros64();
    runTick();
    debugTick();
    uint64_t tickDuration = micros64() - tickStart;
    if (tickDuration < TICK_PERIOD_US) {
        uint64_t remainingTime = TICK_PERIOD_US - tickDuration;
        delayMicroseconds(remainingTime);
    }
}
