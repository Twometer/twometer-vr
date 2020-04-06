#include <iostream>
#include "net/StreamClient.h"

int main() {
    std::cout << "Hello, Virtual World!" << std::endl;

    StreamClient streamClient([](const DataPacket &packet) {
        std::cout << "Received data with " << packet.controllerStates.size() << " controller state frames" << std::endl;
    });
    streamClient.Connect();

    return 0;
}
