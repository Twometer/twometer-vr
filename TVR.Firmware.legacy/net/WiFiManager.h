#include "WiFiConfig.h"

class WiFiManager {
public:
  void connect() {
    WiFi.persistent(true);  // Save those credentials
    WiFi.mode(WIFI_STA);    // Station mode for ESP-firmware glitch prevention
    WiFi.begin(WIFI_SSID, WIFI_PASS); // Connect

    while (WiFi.status() != WL_CONNECTED) {
      delay(100);
    }
  }
};
