# Firmware
This is the TwometerVR firmware utilizing the MPU-9250 IMU chip.

Technically, this is version 2 because there was a previous version that utilized the MPU-6050 IMU, which
was an absolute nightmare to use so I rewrote it. That's why the version in the code say v2.0.

# Usage
To be able to build the Firmware, you need:
- Arduino IDE
- ESP8266 [platform files](https://github.com/esp8266/Arduino#installing-with-boards-manager)
- An ESP8266 programming board
- This ESP8266-modified [SparkFun library](https://github.com/Twometer/SparkFun_MPU-9250-DMP_ESP8266_Library)
- A new file called `WiFiConfig.h` in this directory that contains your WiFi credentials, like this:

```cpp
const String WIFI_SSID = "";
const String WIFI_PASS = "";
```

## Features
- No more yaw drifting due to 9-DOF sensor fusion with the MPU-9250
- Much faster response times and update rates
- More readable, maintainable and cleaner code.