#ifndef TVR_DISCOVERY
#define TVR_DISCOVERY

#include "NetDefs.h"
#include "Packets.h"
#include "UdpClient.h"

class Discovery {
   private:
    UdpClient client;

    IPAddress broadcastIp{};
    IPAddress serverIp{};

    uint8_t incoming[24];

   public:
    IPAddress discover() {
        broadcastIp = ~uint32_t(WiFi.subnetMask()) | uint32_t(WiFi.gatewayIP());
        client.begin(broadcastIp, BROADCAST_PORT);

        while (!runDiscovery()) {
            delay(100);
        }

        return serverIp;
    }

   private:
    bool runDiscovery() {
        Packets::sendDiscovery(client);

        int result = client.readPacket(incoming, 24);
        if (result <= 0)
            return false;  // Did we receive something?
        if (incoming[0] != 0x81)
            return false;  // Is it the 0x81 Discover Reply packet?

        serverIp.fromString((char *)(incoming + 1));
        return true;
    }
};

#endif