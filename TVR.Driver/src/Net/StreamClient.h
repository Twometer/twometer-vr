//
// Created by Twometer on 9 Nov 2020.
//

#ifndef TVR_DRIVER_STREAMCLIENT_H
#define TVR_DRIVER_STREAMCLIENT_H

#include <thread>
#include "UdpClient.h"

constexpr int PORT = 20156;

class StreamClient {
private:
    UdpClient udpClient;

    uint8_t *receiveBuffer;

    std::thread *receiveThread;

    bool threadRunning = true;

public:
    StreamClient();

    ~StreamClient();

    void close();

private:
    void receiveLoop();

};


#endif //TVR_DRIVER_STREAMCLIENT_H
