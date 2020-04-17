//
// Created by twome on 06/04/2020.
//

#include "StreamClient.h"

#include <utility>
#include "Buffer.h"

bool StreamClient::Connect() {
    return tcpClient.Connect("localhost", NetworkPort);
}

void StreamClient::Close() {
    closeRequested = true;
}

int16_t StreamClient::ReadShort() {
    uint8_t data[2];
    tcpClient.Receive(data, 2);

    int16_t value;
    memcpy(&value, data, 2);
    return value;
}

StreamClient::StreamClient(std::function<void(DataPacket)> callback) : callback(std::move(callback)) {
}

void StreamClient::ReceiveLoop() {
    auto *recvBuf = new uint8_t[32767];

    do {
        int packetLen = ReadShort();
        if (packetLen <= 0) continue;

        int received = tcpClient.Receive(recvBuf, packetLen);
        if (received <= 0) return;

        Buffer buffer(recvBuf, received);
        DataPacket packet;

        int controllerStateCount = buffer.Read<uint8_t>();
        for (int i = 0; i < controllerStateCount; i++) {
            auto controllerId = buffer.Read<uint8_t>();
            auto posX = buffer.Read<float>();
            auto posY = buffer.Read<float>();
            auto posZ = buffer.Read<float>();
            auto rotX = buffer.Read<float>();
            auto rotY = buffer.Read<float>();
            auto rotZ = buffer.Read<float>();

            ControllerState state(controllerId, posX, posY, posZ, rotX, rotY, rotZ);

            auto btnCount = buffer.Read<uint8_t>();
            for (int j = 0; j < btnCount; j++) {
                auto btnId = buffer.Read<uint8_t>();
                auto pressed = buffer.Read<uint8_t>() == 1;
                state.buttons[static_cast<Button>(btnId)] = pressed;
            }
            packet.controllerStates.push_back(state);
        }

        callback(packet);
    } while (!closeRequested);
}
