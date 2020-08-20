#ifndef TVR_PACKET
#define TVR_PACKET

#include "UdpClient.h"

class Packet {
  public:
    static void sendStatusPacket(UdpClient& client, byte status) {
      byte data[] = { 0xFF, status };
      client.send(data, 2);
    }

    static void sendDataPacket(UdpClient& client, byte numButtonPresses, byte* buttonPresses, float ax, float ay, float az, float gx, float gy, float gz, float mx, float my, float mz) {
      int16_t packetLen = 1 + 1 + numButtonPresses + 4 * 9;
      byte data[packetLen];
      int offset = 0;

      // Controller id
      copy(data, offset, byte(CONTROLLER_ID));

      // Button presses
      copy(data, offset, numButtonPresses);
      for (int i = 0; i < numButtonPresses; i++) {
        copy(data, offset, buttonPresses[i]);
      }

      // MPU data
      copy(data, offset, ax);
      copy(data, offset, ay);
      copy(data, offset, az);
      copy(data, offset, gx);
      copy(data, offset, gy);
      copy(data, offset, gz);
      copy(data, offset, mx);
      copy(data, offset, my);
      copy(data, offset, mz);

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
