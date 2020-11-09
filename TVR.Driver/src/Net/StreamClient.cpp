//
// Created by Twometer on 9 Nov 2020.
//

#include "StreamClient.h"
#include "Buffer.h"

StreamClient::StreamClient() {
    udpClient.bind(PORT);

    receiveBuffer = new uint8_t[4096];
    receiveThread = new std::thread([this] { receiveLoop(); });
}

StreamClient::~StreamClient() {
    delete receiveThread;
}

void StreamClient::receiveLoop() {
    while (threadRunning) {
        size_t received = udpClient.receive(receiveBuffer, 4096);
        Buffer buffer(receiveBuffer, received);

        auto packetId = buffer.get<uint8_t>();
        // TODO parse packet
    }
}

void StreamClient::close() {
    threadRunning = false;
    receiveThread->join();
}
