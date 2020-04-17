//
// Created by twome on 06/04/2020.
//

#ifndef TVRDRV_BUFFER_H
#define TVRDRV_BUFFER_H


#include <cstdint>
#include <algorithm>

class Buffer {
private:
    uint8_t *data;

    int dataLen;

    int offset;

public:
    Buffer(uint8_t *data, int len);

    template<typename T>
    T Read() {
        T val;
        memcpy(&val, data + offset, sizeof(val));
        offset += sizeof(val);
        return val;
    }

};


#endif //TVRDRV_BUFFER_H
