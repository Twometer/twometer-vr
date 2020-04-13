//
// Created by twome on 13/04/2020.
//

#include <iostream>
#include "net/StreamClient.h"

int main() {
    auto streamClient = new StreamClient([](const DataPacket &dataPacket) {
        std::cout << "Received packet from server" << std::endl;
    });

    if (!streamClient->Connect())
        std::cerr << "Failed to connect" << std::endl;

    streamClient->ReceiveLoop();

    return 0;
}