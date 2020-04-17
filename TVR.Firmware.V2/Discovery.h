#include <WiFiUdp.h>

class Discovery {
private:
  WiFiUDP udpClient;
  String serverIp;
  byte discoverySequence[4] = { 0x79, 0x65, 0x65, 0x74 };
  char udp_incoming[15];
  
public:
  String discover() {
    IPAddress broadcastIp = ~uint32_t(WiFi.subnetMask()) | uint32_t(WiFi.gatewayIP());
    udpClient.begin(DISCOVERY_PORT);

    while (!sendRequest) {
      delay(500);
    }
  }

private:
  bool sendRequest() {
    udp.beginPacket(broadcastIp, DISCOVERY_PORT);
    udp.write(discoverySequence, 4);
    udp.endPacket();

    int packetSize = udp.parsePacket();
    if (!packetSize)
      return false;

    int len = udp.read(udp_incoming, packetSize);
    if (len <= 0)
      return false;

    serverIp = String(udp_incoming);
    return true;
  }

};
