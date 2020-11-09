//
// Created by Twometer on 9 Nov 2020.
//

#ifndef TVR_DRIVER_TRACKERCOLOR_H
#define TVR_DRIVER_TRACKERCOLOR_H

enum class TrackerColor
{
    Invalid = 0x00,
    Red = 0x01,     // Left hand
    Blue = 0x02,    // Right hand
    Green = 0x03,   // Left leg
    Magenta = 0x04, // Right leg
    Cyan = 0x05,    // Hips
    Yellow = 0x06   // Generic
};

#endif //TVR_DRIVER_TRACKERCOLOR_H
