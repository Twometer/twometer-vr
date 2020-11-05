#ifndef TVR_DISCOVERY
#define TVR_DISCOVERY

#include <WiFiUdp.h>

class Discovery {
private:
  WiFiUDP udpClient;

  IPAddress serverIp;
  IPAddress broadcastIp;

  byte discoverySequence[4] = { 0x79, 0x65, 0x65, 0x74 };
  char udpIncoming[15];

public:
  IPAddress discover() {
    broadcastIp = ~uint32_t(WiFi.subnetMask()) | uint32_t(WiFi.gatewayIP());
    udpClient.begin(DISCOVERY_PORT);

    while (!sendRequest()) {
      delay(100);
    }
    return serverIp;
  }

private:
  bool sendRequest() {
    udpClient.beginPacket(broadcastIp, DISCOVERY_PORT);
    udpClient.write(discoverySequence, 4);
    udpClient.endPacket();

    int packetSize = udpClient.parsePacket();
    if (!packetSize)
      return false;

    int len = udpClient.read(udpIncoming, packetSize);
    if (len <= 0)
      return false;

    serverIp = IPAddress();
    serverIp.fromString(udpIncoming);
    return true;
  }

};

#endif // TVR_DISCOVERY
