#ifndef TVR_TRACKER_COLOR
#define TVR_TRACKER_COLOR

enum TrackerColor
{
    Invalid = 0x00,
    Red = 0x01,     // Left hand
    Blue = 0x02,    // Right hand
    Green = 0x03,   // Left leg
    Magenta = 0x04, // Right leg
    Cyan = 0x05,    // Hips
    Yellow = 0x06   // Generic
};

#endif