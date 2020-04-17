//
// Created by twome on 15/04/2020.
//

#ifndef OPENVR_MATH_H
#define OPENVR_MATH_H

#include <cmath>

// Quaternion Math from
// https://github.com/matzman666/OpenVR-InputEmulator/blob/master/lib_vrinputemulator/include/openvr_math.h

// Copyright matzman666
// This file is licensed under the GNU General Public License v3.0
// Thank you :)


inline vr::HmdQuaternion_t operator*(const vr::HmdQuaternion_t &lhs, const vr::HmdQuaternion_t &rhs) {
    return {
            (lhs.w * rhs.w) - (lhs.x * rhs.x) - (lhs.y * rhs.y) - (lhs.z * rhs.z),
            (lhs.w * rhs.x) + (lhs.x * rhs.w) + (lhs.y * rhs.z) - (lhs.z * rhs.y),
            (lhs.w * rhs.y) + (lhs.y * rhs.w) + (lhs.z * rhs.x) - (lhs.x * rhs.z),
            (lhs.w * rhs.z) + (lhs.z * rhs.w) + (lhs.x * rhs.y) - (lhs.y * rhs.x)
    };
}

namespace vrmath {

    inline vr::HmdQuaternion_t quaternionFromRotationX(double rot) {
        auto ha = rot / 2;
        return {
                std::cos(ha),
                std::sin(ha),
                0.0f,
                0.0f
        };
    }

    inline vr::HmdQuaternion_t quaternionFromRotationY(double rot) {
        auto ha = rot / 2;
        return {
                std::cos(ha),
                0.0f,
                std::sin(ha),
                0.0f
        };
    }

    inline vr::HmdQuaternion_t quaternionFromRotationZ(double rot) {
        auto ha = rot / 2;
        return {
                std::cos(ha),
                0.0f,
                0.0f,
                std::sin(ha)
        };
    }

    inline vr::HmdQuaternion_t quaternionFromYawPitchRoll(double yaw, double pitch, double roll) {
        return quaternionFromRotationY(yaw) * quaternionFromRotationX(pitch) * quaternionFromRotationZ(roll);
    }

}

#endif //OPENVR_MATH_H
