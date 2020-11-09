cmake_minimum_required(VERSION 3.17)
project(TVR_Driver)

set(CMAKE_CXX_STANDARD 14)

add_subdirectory(lib/openvr)

add_library(TVR_Driver SHARED
        src/main.cpp src/Driver/WatchdogDriver.cpp src/Driver/WatchdogDriver.h src/Driver/TrackerProvider.cpp src/Driver/TrackerProvider.h src/Utils/OpenVrExports.h src/Net/UdpClient.cpp src/Net/UdpClient.h src/Net/StreamClient.cpp src/Net/StreamClient.h src/Model/Vectors.h src/Model/TrackerClass.h src/Model/TrackerState.h src/Net/Buffer.cpp src/Net/Buffer.h src/Model/TrackerColor.h)

target_include_directories(TVR_Driver PRIVATE lib/openvr/headers)
target_link_libraries(TVR_Driver Ws2_32.lib)

# Format output file name for OpenVR
set_target_properties(TVR_Driver PROPERTIES PREFIX "driver_")
set_target_properties(TVR_Driver PROPERTIES OUTPUT_NAME "tvr")