#ifndef TVR_PACKET
#define TVR_PACKET

#include "UdpClient.h"

class Packet {
  public:
    static void sendStatusPacket(UdpClient& client, byte status) {
      byte data[] = { 0xFF, status };
      client.send(data, 2);
    }

    static void sendDataPacket(UdpClient& client, byte numButtonPresses, byte* buttonPresses, float qx, float qy, float qz, float qw) {
      int16_t packetLen = 1 + 1 + numButtonPresses + 4 * 4;
      byte data[packetLen];
      int offset = 0;

      // Controller id
      copy(data, offset, byte(CONTROLLER_ID));

      // Button presses
      copy(data, offset, numButtonPresses);
      for (int i = 0; i < numButtonPresses; i++) {
        copy(data, offset, buttonPresses[i]);
      }

      // Quaternion rotations
      copy(data, offset, qx);
      copy(data, offset, qy);
      copy(data, offset, qz);
      copy(data, offset, qw);

      client.send(data, packetLen);
    }

  private:
    template<typename T>
    static void copy(byte* dst, int &offset, T data) {
      memcpy(dst + offset, &data, sizeof(data));
      offset += sizeof(data);
    }
};

#endif // TVR_PACKET
