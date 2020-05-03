class Packet {
  public:
    static void SendCalibrationComplete(WiFiUDP *udp, IPAddress& serverIp) {
      byte data[] = { 0xFF, 0xFF };
      udp->beginPacket(serverIp, CONTROLLER_PORT);
      udp->write(data, 2);
      udp->endPacket();
    }

    static void Send(WiFiUDP *udp, IPAddress& serverIp, byte numButtonPresses, byte* buttonPresses, float yaw, float pitch, float roll) {
      int16_t packetLen = 1 + 1 + (numButtonPresses) + 4 * 3;
      byte data[packetLen];
      int offset = 0;

      copy(data, offset, byte(CONTROLLER_ID));    // Controller id

      copy(data, offset, numButtonPresses);      // Button presses
      for (int i = 0; i < numButtonPresses; i++) {
        copy(data, offset, buttonPresses[i]);
      }

      // Rotations
      copy(data, offset, yaw);
      copy(data, offset, pitch);
      copy(data, offset, roll);

      udp->beginPacket(serverIp, CONTROLLER_PORT);
      udp->write(data, packetLen);
      udp->endPacket();
    }

  private:
    template<typename T>
    static void copy(byte* dst, int &offset, T data) {
      memcpy(dst + offset, &data, sizeof(data));
      offset += sizeof(data);
    }
};
