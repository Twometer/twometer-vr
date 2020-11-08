# Driver
OpenVR driver implementation which connects the TVR service to games



### Installing

The release installation package will automatically install the driver for you. If you want to do it yourself, see the next section.



### Building from source

You will need:

- CLion or other CMake compatible IDE
- The submodules for this repo to get `.\lib\openvr`
- The MSVC compiler and Windows



Building:

- Run a CMake release build. It should automatically create the `driver_vr.dll` file required by SteamVR.
- Copy the dll file to `.\ovr\tvr\bin\win64\driver_tvr.dll`.



Installing: Run `.\ovr\Install.ps1`