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
    T Get() {
        T val;
        size_t valSize = sizeof(val);

        if (offset >= size) {
            throw std::out_of_range("Buffer access out of range");
        }

        memcpy(&val, data + offset, valSize);
        offset += valSize;

        return val;
    }

    std::string GetString();
};


#endif //TVR_DRIVER_BUFFER_H
