//
// Created by twome on 06/04/2020.
//

#include "StreamClient.h"

#include <utility>
#include "Buffer.h"

void StreamClient::Connect() {
    if (!tcpClient.Connect("localhost", NetworkPort))
        return;

    auto *recvBuf = new uint8_t[32767];
    memset(recvBuf, 0, 32767);

    do {
        int packetLen = ReadInt();
        if (packetLen <= 0) continue;

        int received = tcpClient.Receive(recvBuf, packetLen);
        if (received <= 0) return;

        Buffer buffer(recvBuf, received);
        DataPacket packet;

        int controllerStateCount = buffer.Read<int32_t>();
        for (int i = 0; i < controllerStateCount; i++) {
            auto controllerId = buffer.Read<int32_t>();
            auto posX = buffer.Read<float>();
            auto posY = buffer.Read<float>();
            auto posZ = buffer.Read<float>();
            auto rotX = buffer.Read<float>();
            auto rotY = buffer.Read<float>();
            packet.controllerStates.emplace_back(controllerId, posX, posY, posZ, rotX, rotY);
        }

        int buttonPressCount = buffer.Read<int32_t>();
        for (int i = 0; i < buttonPressCount; i++) {
            auto controllerId = buffer.Read<int32_t>();
            auto buttonId = buffer.Read<int32_t>();
            packet.buttonPresses.emplace_back(controllerId, buttonId);
        }

        callback(packet);
    } while (!closeRequested);
}

void StreamClient::Close() {
    closeRequested = true;
}

int32_t StreamClient::ReadInt() {
    uint8_t data[4];
    tcpClient.Receive(data, 4);

    int32_t value;
    memcpy(&value, data, 4);
    return value;
}

StreamClient::StreamClient(std::function<void(DataPacket)> callback) : callback(std::move(callback)) {
}
