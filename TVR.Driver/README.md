# Driver
The TwometerVR driver that routes the controllers' input to SteamVR.

## Installing
Download the TwometerVR package from GitHub Downloads. In the `drivers` folder (or the drivers folder in this repository), run `Install.ps1`.

## Building from Source
The driver is written with CLion as an IDE, but you should be able to use any other IDE that can read CMake projects.

The CMake project is configured so that it outputs the correct `driver_tvr.dll` file.

To make use of this file, copy it to `drivers\tvr\bin\win64\driver_tvr.dll` and run `Install.ps1` or `Reinstall.ps1`, depending on whether you already registered it with SteamVR.

> Note: Supports only Windows. Tested and working on MinGW/gcc.