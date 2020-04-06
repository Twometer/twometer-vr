//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_STREAMCLIENT_H
#define TVRDRV_STREAMCLIENT_H

#include <functional>
#include "TcpClient.h"
#include "../model/DataPacket.h"

constexpr int NetworkPort = 12741;

class StreamClient {
private:
    TcpClient tcpClient{};

    volatile bool closeRequested = false;

    int32_t ReadInt();

    std::function<void(DataPacket)> callback;

public:
    explicit StreamClient(std::function<void(DataPacket)> callback);

    void Connect();

    void Close();

};


#endif //TVRDRV_STREAMCLIENT_H
