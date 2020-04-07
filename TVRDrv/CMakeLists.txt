cmake_minimum_required(VERSION 3.15)
project(TVRDrv)

set(CMAKE_CXX_STANDARD 14)

add_executable(TVRDrv src/main.cpp src/model/ButtonPress.h src/model/ControllerState.h src/model/DataPacket.h src/net/StreamClient.cpp src/net/StreamClient.h src/net/TcpClient.cpp src/net/Buffer.cpp src/net/Buffer.h src/util/Logger.h)

target_link_libraries(TVRDrv Ws2_32.lib)