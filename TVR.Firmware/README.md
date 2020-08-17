# Firmware
This is the firmware that runs on the ESP8266s (the controllers' main chips)

## Usage
To build and install the Firmware, you need:
- The Arduino IDE
- ESP8266 [platform files](https://github.com/esp8266/Arduino#installing-with-boards-manager)
- An ESP8266 programming board
- This MPU-9250 [library](https://github.com/Twometer/MPU9250) for software AHRS
  or [this one](https://github.com/Twometer/SparkFun_MPU-9250-DMP_ESP8266_Library) for hardware AHRS
- A new file called `WiFiConfig.h` in the `net` directory that contains your WiFi credentials, like this:

```cpp
const String WIFI_SSID = "<ssid_here>";
const String WIFI_PASS = "<pw_here>";
```

> Replace `<ssid_here>` and `<pw_here>` with your SSID and password, accordingly.

More information can be found in the project wiki.

## Features
- No drifting due to 9-DOF sensor fusion with the MPU-9250 (if you use software AHRS)
- Fast response times and high update rates using UDP streams
- Quick startup and automatic discovery of the service
- Readable, maintainable and clean codebase.

## A note on the version numbers
Technically, this is version 1 because there never was a release. However, I made a previous version that utilized the MPU-6050 IMU, which was an absolute nightmare to use so I rewrote it using the MPU-9250 IMU chip. It is completely incompatible to the old version, which is why the version in the code says `v2.x`.