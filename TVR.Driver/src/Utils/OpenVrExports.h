//
// Created by Twometer on 8 Nov 2020.
//

#ifndef TVR_DRIVER_OPENVREXPORTS_H
#define TVR_DRIVER_OPENVREXPORTS_H

#if defined(_WIN32)
#define HMD_DLL_EXPORT extern "C" __declspec( dllexport )
#define HMD_DLL_IMPORT extern "C" __declspec( dllimport )
#elif defined(__GNUC__) || defined(COMPILER_GCC) || defined(__APPLE__)
#define HMD_DLL_EXPORT extern "C" __attribute__((visibility("default")))
#define HMD_DLL_IMPORT extern "C"
#else
#error "Unsupported Platform."
#endif

#endif //TVR_DRIVER_OPENVREXPORTS_H
