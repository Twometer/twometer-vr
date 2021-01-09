//
// Created by Twometer on 9 Nov 2020.
//

#ifndef TVR_DRIVER_UDPCLIENT_H
#define TVR_DRIVER_UDPCLIENT_H

#include <cstdint>

#include <WinSock2.h>

class UdpClient {
private:
    SOCKET sock;

    sockaddr_in serverAddress{};

public:
    UdpClient(const char *ip, uint16_t port);

    ~UdpClient();

    int Receive(uint8_t *buffer, int len) const;

    void Send(const uint8_t *buffer, int len) const;
};


#endif //TVR_DRIVER_UDPCLIENT_H
