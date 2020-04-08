cmake_minimum_required(VERSION 3.15)
project(TVRDrv)

set(CMAKE_CXX_STANDARD 14)

add_subdirectory(lib/openvr)

add_executable(TVRDrv
        src/net/StreamClient.cpp
        src/net/TcpClient.cpp
        src/net/Buffer.cpp
        src/main.cpp src/driver/SteamVRDriver.cpp src/driver/SteamVRDriver.h)

target_include_directories(TVRDrv PRIVATE lib/openvr/headers)
target_link_libraries(TVRDrv Ws2_32.lib)