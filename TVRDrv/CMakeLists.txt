cmake_minimum_required(VERSION 3.15)
project(TVRDrv)

set(CMAKE_CXX_STANDARD 14)

add_subdirectory(lib/openvr)

add_executable(TVRDrv
        src/net/StreamClient.cpp
        src/net/TcpClient.cpp
        src/net/Buffer.cpp
        src/main.cpp src/driver/ServerDriver.cpp src/driver/ServerDriver.h src/driver/ControllerDriver.cpp src/driver/ControllerDriver.h src/driver/WatchdogDriver.cpp src/driver/WatchdogDriver.h)

target_include_directories(TVRDrv PRIVATE lib/openvr/headers)
target_link_libraries(TVRDrv Ws2_32.lib)