#ifndef TVR_POSE_INPUT
#define TVR_POSE_INPUT

#include <ICM-20948.h>
#include "Hardware.h"

class PoseInput
{
public:
    void setup();
    void update();
    vec4 &getPose();
};

#endif