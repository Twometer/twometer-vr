cmake_minimum_required(VERSION 3.15)
project(TVRDrv)

set(CMAKE_CXX_STANDARD 14)

add_subdirectory(lib/openvr)

add_library(TVRDrv SHARED
        src/main.cpp
        src/net/StreamClient.cpp
        src/net/TcpClient.cpp
        src/net/Buffer.cpp
        src/driver/ServerDriver.cpp
        src/driver/ControllerDriver.cpp
        src/driver/WatchdogDriver.cpp
        src/model/ControllerState.cpp src/util/openvr_math.h)

add_executable(TVRDrvTest
        src/main_test.cpp
        src/net/StreamClient.cpp
        src/net/TcpClient.cpp
        src/net/Buffer.cpp
        src/driver/ServerDriver.cpp
        src/driver/ControllerDriver.cpp
        src/driver/WatchdogDriver.cpp
        src/model/ControllerState.cpp src/main_test.cpp)

set_target_properties(TVRDrv PROPERTIES PREFIX "driver_")
set_target_properties(TVRDrv PROPERTIES OUTPUT_NAME "tvr")

target_include_directories(TVRDrv PRIVATE lib/openvr/headers)
target_link_libraries(TVRDrv Ws2_32.lib)

target_include_directories(TVRDrvTest PRIVATE lib/openvr/headers)
target_link_libraries(TVRDrvTest Ws2_32.lib)