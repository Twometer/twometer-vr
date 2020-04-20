#include <WiFiUdp.h>

class Discovery {
private:
  WiFiUDP udpClient;
  
  String serverIp;
  IPAddress broadcastIp;
  
  byte discoverySequence[4] = { 0x79, 0x65, 0x65, 0x74 };
  char udpIncoming[15];
  
public:
  String discover() {
    broadcastIp = ~uint32_t(WiFi.subnetMask()) | uint32_t(WiFi.gatewayIP());
    udpClient.begin(DISCOVERY_PORT);

    while (!sendRequest()) {
      delay(500);
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

    serverIp = String(udpIncoming);
    return true;
  }

};
