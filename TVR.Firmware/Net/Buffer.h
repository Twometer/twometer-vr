#ifndef TVR_BUFFER
#define TVR_BUFFER

#include <stdint.h>

class Buffer
{
private:
    uint8_t *buffer;
    size_t size;
    size_t offset;

public:
    Buffer(size_t size) : buffer(new uint8_t[size]), size(size), offset(0)
    {
    }

    ~Buffer()
    {
        delete[] buffer;
    }

    template <typename T>
    void put(T data)
    {
        size_t size = sizeof(data);
        memcpy(buffer + offset, &data, size);
        offset += size;
    }

    void put(uint8_t *data, size_t size)
    {
        memcpy(buffer + offset, data, size);
        offset += size;
    }

    uint8_t *get(int offset)
    {
        return buffer + offset;
    }
};

#endif