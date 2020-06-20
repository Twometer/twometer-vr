# Driver
This is the driver that registers the TwometerVR controllers with SteamVR, routes the inputs accordingly and provides
the binding UI in Steam.

## Installing
The driver is automatically installed with the main installation package. If you want to build and install it yourself, see the next section.

## Building from Source
I wrote this driver using the CLion IDE, so that's the used project format. However, as it is basically CMake underneath, you should be able to use any IDE (or just CMake itself). Just make sure that you cloned all submodules, so that you have the `lib/openvr` library which is required in order to build the driver.

SteamVR expects a driver file that has a special naming format. The CMake project is configured so that it automatically builds the correct `driver_vr.dll` file. Before installing the driver, copy it to `.\drivers\tvr\bin\win64\driver_tvr.dll` in this folder.

Now, we can install the driver: The `drivers` folder contains the required folder structure for SteamVR. Just run `Install.ps1`, and when starting SteamVR the next time you should be able to use the TwometerVR controllers. If you changed something and want to deregister and reregister the driver file quickly, use `Reinstall.ps1` to update all changes, if required.

> Note: Currently only supports Windows. Tested on MinGW/gcc.