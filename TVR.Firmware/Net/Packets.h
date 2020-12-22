#ifndef TVR_PACKETS
#define TVR_PACKETS

#include <IcmVectors.h>

#include "Buffer.h"
#include "UdpClient.h"
#include "NetDefs.h"
#include "../Model/TrackerClass.h"
#include "../Model/TrackerColor.h"

class Packets
{
public:
    static void sendDiscovery(UdpClient &client)
    {
        client.beginPacket();
        client.write(0x80);        // Packet ID
        client.write(UNIVERSE_ID); // Universe ID
        client.endPacket();
    }

    static void sendHello(UdpClient &client, TrackerClass trackerClass, TrackerColor trackerColor, String serialNo)
    {
        client.beginPacket();
        client.write(0x82);
        client.write((uint8_t)trackerClass);
        client.write((uint8_t)trackerColor);
        client.writeRaw(serialNo.c_str(), serialNo.length() + 1);
        client.endPacket();
    }

    static void receiveHello(UdpClient &client, uint8_t &trackerId)
    {
        int result;
        uint8_t data[2];
        while (true)
        {
            result = client.readPacket(data, 2);
            if (result <= 0 || data[0] != 0x83)
            {
                delay(100);
                continue;
            }

            trackerId = data[1];
            break;
        }
    }

    static void sendState(UdpClient &client, uint8_t trackerId, uint16_t buttons, vec4 rotation)
    {
        client.beginPacket();
        client.write(trackerId);
        client.write(buttons);
        client.write(rotation.x);
        client.write(rotation.y);
        client.write(rotation.z);
        client.write(rotation.w);
        client.endPacket();
    }
};

#endif