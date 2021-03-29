# Firmware
Source code for the TwometerVR firmware that runs on the ESP8266 chips.

## Building
You need:

- Arduino IDE (used as an upload tool)
- ESP8266 [platform files](https://github.com/esp8266/Arduino#installing-with-boards-manager)
- ICM-20948 [Arduino library](https://github.com/Twometer/ICM-20948-Arduino)
- An ESP8266 programming device

## Recommended upload settings
- CPU Speed: 160 MHz
- Espressif Version: 2.2.1+100

## Features
- ICM-20948 IMUs with DMP support for high accuracy
- Automatic server discovery and runtime IMU calibration
- Supports up to 8 button inputs (16 with a larger Shift Register)