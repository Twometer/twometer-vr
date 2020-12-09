//
// Created by Twometer on 9 Nov 2020.
//

#ifndef TVR_DRIVER_BUFFER_H
#define TVR_DRIVER_BUFFER_H

#include <cstdint>
#include <stdexcept>

class Buffer {
private:
    uint8_t *data;
    size_t size;
    size_t offset;

public:
    Buffer(uint8_t *data, size_t size);

    template<typename T>
    T get() {
        T val;
        size_t valSize = sizeof(val);

        memcpy(&val, data + offset, valSize);
        offset += valSize;

        if (offset >= size) {
            throw std::out_of_range("Buffer access out of range");
        }

        return val;
    }

    std::string getString();
};


#endif //TVR_DRIVER_BUFFER_H
