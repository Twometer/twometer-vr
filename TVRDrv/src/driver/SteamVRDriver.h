//
// Created by twome on 08/04/2020.
//

#ifndef TVRDRV_STEAMVRDRIVER_H
#define TVRDRV_STEAMVRDRIVER_H

#include <openvr_driver.h>

#if defined(_WIN32)
#define HMD_DLL_EXPORT extern "C" __declspec( dllexport )
#define HMD_DLL_IMPORT extern "C" __declspec( dllimport )
#elif defined(__GNUC__) || defined(COMPILER_GCC) || defined(__APPLE__)
#define HMD_DLL_EXPORT extern "C" __attribute__((visibility("default")))
#define HMD_DLL_IMPORT extern "C"
#else
#error "Unsupported Platform."
#endif

class SteamVRDriver {

};


#endif //TVRDRV_STEAMVRDRIVER_H
