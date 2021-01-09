//
// Created by Twometer on 9 Nov 2020.
//

#include <system_error>
#include <iostream>
#include "UdpClient.h"

UdpClient::UdpClient(const char *ip, uint16_t port) {
    WSAData data{};
    int ret = WSAStartup(MAKEWORD(2, 2), &data);
    if (ret != 0)
        throw std::system_error(WSAGetLastError(), std::system_category(), "WSAStartup Failed");

    sock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (sock == INVALID_SOCKET)
        throw std::system_error(WSAGetLastError(), std::system_category(), "Error opening socket");

    serverAddress.sin_family = AF_INET;
    serverAddress.sin_port = htons(port);
    serverAddress.sin_addr.S_un.S_addr = inet_addr(ip);
}

UdpClient::~UdpClient() {
    closesocket(sock);
    WSACleanup();
}

int UdpClient::Receive(uint8_t *buffer, int len) const {
    int fromSize = sizeof(serverAddress);
    return recvfrom(sock, (char *) buffer, len, 0, (sockaddr *) &serverAddress, &fromSize);
}

void UdpClient::Send(const uint8_t *buffer, int len) const {
    int ret = sendto(sock, (const char *) buffer, len, 0, (const sockaddr *) &serverAddress, sizeof(serverAddress));
    if (ret < 0)
        throw std::system_error(WSAGetLastError(), std::system_category(), "sendto failed");
}
