cmake_minimum_required(VERSION 3.15)
project(TVR.Driver)

set(CMAKE_CXX_STANDARD 14)

add_subdirectory(lib/openvr)

add_library(TVR.Driver SHARED
        src/main.cpp
        src/net/StreamClient.cpp
        src/net/TcpClient.cpp
        src/net/Buffer.cpp
        src/driver/ServerDriver.cpp
        src/driver/ControllerDriver.cpp
        src/driver/WatchdogDriver.cpp
        src/model/ControllerState.cpp)

add_executable(TVR.Driver.Test
        src/main_test.cpp
        src/net/StreamClient.cpp
        src/net/TcpClient.cpp
        src/net/Buffer.cpp
        src/driver/ServerDriver.cpp
        src/driver/ControllerDriver.cpp
        src/driver/WatchdogDriver.cpp
        src/model/ControllerState.cpp
        src/main_test.cpp)

# Format output file name to work with SteamVR
set_target_properties(TVR.Driver PROPERTIES PREFIX "driver_")
set_target_properties(TVR.Driver PROPERTIES OUTPUT_NAME "tvr")

# Includes for the main project
target_include_directories(TVR.Driver PRIVATE lib/openvr/headers)
target_link_libraries(TVR.Driver Ws2_32.lib)

# Includes for the test project
target_include_directories(TVR.Driver.Test PRIVATE lib/openvr/headers)
target_link_libraries(TVR.Driver.Test Ws2_32.lib)