//
// Created by Twometer on 20/09/2019.
//

#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wunknown-pragmas"    // Without this, it warns at the pop
#pragma ide diagnostic ignored "hicpp-signed-bitwise"   // We can't fix the signed-bitwise OPs of Windows' headers

#include "TcpClient.h"
#include "../util/Logger.h"

#include <string>

bool TcpClient::Connect(const char *host, unsigned short port) {
    log::info << "Connecting to " << host << ":" << port << log::endl;

    WSADATA wsaData;
    auto ConnectSocket = INVALID_SOCKET;
    struct addrinfo *result = nullptr, *ptr = nullptr, hints{};

    int iResult;

    // Initialize Winsock
    iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (iResult != 0) {
        log::err << "WSAStartup failed with error " << iResult << log::endl;
        return false;
    }

    hints.ai_family = AF_UNSPEC;
    hints.ai_socktype = SOCK_STREAM;
    hints.ai_protocol = IPPROTO_TCP;

    iResult = getaddrinfo(host, std::to_string(port).c_str(), &hints, &result);
    if (iResult != 0) {
        log::err << "getaddrinfo failed with error " << iResult << log::endl;
        WSACleanup();
        return false;
    }

    // Attempt to connect to an address until one succeeds
    for (ptr = result; ptr != nullptr; ptr = ptr->ai_next) {
        // Create a SOCKET for connecting to server
        ConnectSocket = socket(ptr->ai_family, ptr->ai_socktype, ptr->ai_protocol);
        if (ConnectSocket == INVALID_SOCKET) {
            log::err << "Socket failed with error " << WSAGetLastError() << log::endl;
            WSACleanup();
            return false;
        }

        // Connect to server.
        iResult = connect(ConnectSocket, ptr->ai_addr, (int) ptr->ai_addrlen);
        if (iResult == SOCKET_ERROR) {
            closesocket(ConnectSocket);
            ConnectSocket = INVALID_SOCKET;
            continue;
        }
        break;
    }

    freeaddrinfo(result);

    if (ConnectSocket == INVALID_SOCKET) {
        log::err << "Failed to connect to the server" << log::endl;
        WSACleanup();
        return false;
    }

    log::info << "Connected." << log::endl;

    this->tcpSocket = ConnectSocket;
    return true;
}

void TcpClient::Close() {
    closesocket(tcpSocket);
    WSACleanup();
}

int TcpClient::Receive(uint8_t *buf, int len) {
    int read = 0;
    int c = 0;
    while (read < len) {
        c = recv(tcpSocket, (char *) (buf + read), len - read, 0);
        if (c < 0)
            return c;
        read += c;
    }
    return read > 0 ? read : -1;
}

void TcpClient::Send(uint8_t *buf, int len) {
    int result = send(tcpSocket, (char *) buf, len, 0);
    if (result == SOCKET_ERROR)
        Close();
}

uint8_t TcpClient::ReadByte() {
    uint8_t buf;
    recv(tcpSocket, (char *) &buf, 1, 0);
    return buf;
}

#pragma clang diagnostic pop