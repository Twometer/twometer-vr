//
// Created by Twometer on 9 Nov 2020.
//

#include <system_error>
#include "UdpClient.h"

UdpClient::UdpClient() {
    WSAData data{};
    int ret = WSAStartup(MAKEWORD(2, 2), &data);
    if (ret != 0)
        throw std::system_error(WSAGetLastError(), std::system_category(), "WSAStartup Failed");

    sock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (sock == INVALID_SOCKET)
        throw std::system_error(WSAGetLastError(), std::system_category(), "Error opening socket");
}

UdpClient::~UdpClient() {
    closesocket(sock);
    WSACleanup();
}

void UdpClient::bind(uint16_t port) const {
    sockaddr_in address{};
    address.sin_family = AF_INET;
    address.sin_addr.s_addr = htonl(INADDR_ANY);
    address.sin_port = htons(port);

    int ret = ::bind(sock, (SOCKADDR*)&address, sizeof(address));
    if (ret < 0)
        throw std::system_error(WSAGetLastError(), std::system_category(), "bind failed");
}

int UdpClient::receive(uint8_t *buffer, int len, int flags) const {
    sockaddr_in from{};
    int fromSize = sizeof(from);

    int ret = recvfrom(sock, (char *) buffer, len, flags, (SOCKADDR *) &from, &fromSize);
    if (ret < 0)
        throw std::system_error(WSAGetLastError(), std::system_category(), "recvfrom failed");

    return ret;
}
