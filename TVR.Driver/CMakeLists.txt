cmake_minimum_required(VERSION 3.17)
project(TVR_Driver)

set(CMAKE_CXX_STANDARD 14)

add_subdirectory(lib/openvr)

add_library(TVR_Driver SHARED
        src/main.cpp src/Driver/WatchdogDriver.cpp src/Driver/WatchdogDriver.h src/Driver/TrackerDriver.cpp src/Driver/TrackerDriver.h src/Utils/OpenVrExports.h)

target_include_directories(TVR_Driver PRIVATE lib/openvr/headers)
target_link_libraries(TVR_Driver Ws2_32.lib)

# Format output file name for OpenVR
set_target_properties(TVR_Driver PROPERTIES PREFIX "driver_")
set_target_properties(TVR_Driver PROPERTIES OUTPUT_NAME "tvr")