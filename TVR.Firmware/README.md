# Firmware
Source code for the TwometerVR firmware that runs on the ESP8266 chips.



### Building

You need:

- Arduino IDE (only used as upload tool)
- ESP8266 [platform files](https://github.com/esp8266/Arduino#installing-with-boards-manager)
- ICM-20948 IMU [library](https://github.com/Twometer/ICM-20948-Arduino)
- ESP8266 programmer



### Features

- High rotational accuracy and no yaw drift with the new ICM-20948 IMUs
- Better way of connecting to the WiFi network (no more hard-coded credentials)
- Automatic server discovery and IMU calibration
- Supports many more controller inputs (buttons)

