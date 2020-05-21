//
// Created by twome on 13/04/2020.
//

#include <iostream>
#include "net/StreamClient.h"

int main() {
    auto streamClient = new StreamClient([](const DataPacket &dataPacket) {
        for (auto &d : dataPacket.controllerStates) {
            auto a_pressed = d.buttons.at(Button::A);
            std::cout << (int) d.controllerId << " " << d.posX << " " << d.posY << " " << d.posZ << std::endl;
            std::cout << (int) d.controllerId << " " << d.qx << " " << d.qy << " " << d.qz << " " << d.qw << std::endl;
        }
    });

    if (!streamClient->Connect())
        std::cerr << "Failed to connect" << std::endl;

    streamClient->ReceiveLoop();

    return 0;
}