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

public:
    UdpClient();

    ~UdpClient();

    void bind(uint16_t port) const;

    int receive(uint8_t* buffer, int len, int flags = 0) const;
};


#endif //TVR_DRIVER_UDPCLIENT_H
