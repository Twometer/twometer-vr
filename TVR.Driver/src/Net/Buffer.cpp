//
// Created by Twometer on 9 Nov 2020.
//

#include "Buffer.h"
#include "../Utils/Constants.h"

Buffer::Buffer(uint8_t *data, size_t size) : data(data), size(size), offset(0) {
}

std::string Buffer::getString() {
    const char *ptr = (const char *) (data + offset);
    int len = strlen(ptr);
    if (len > MAX_PACKET_LEN) {
        return "invalid";
    }

    char *string = new char[len + 1];
    strcpy_s(string, len, ptr);

    return std::string(string);
}
