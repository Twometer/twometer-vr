#ifndef TVR_UDPCLIENT
#define TVR_UDPCLIENT

#include <WiFiUdp.h>

class UdpClient {
private:
  WiFiUDP client{};

  IPAddress serverIp;
  int serverPort;

public:
  void begin(IPAddress ip, int port) {
    serverIp = ip;
    serverPort = port;
    if (!client.begin(port)) {
      Serial.println("Can't initialize UDP client, rebooting...");
      ESP.restart();
    }
  }

  void send(uint8_t* data, int dataLen) {
    client.beginPacket(serverIp, serverPort);
    client.write(data, dataLen);
    client.endPacket();
  }
};

#endif
