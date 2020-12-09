//
// Created by Twometer on 9 Nov 2020.
//

#include <iostream>

#include "StreamClient.h"
#include "Buffer.h"
#include "../Utils/Constants.h"

StreamClient::StreamClient() {
    udpClient.bind(NET_PORT);

    receiveBuffer = new uint8_t[MAX_PACKET_LEN];
    receiveThread = new std::thread([this] { receiveLoop(); });
}

StreamClient::~StreamClient() {
    delete receiveThread;
    for (auto tracker : trackers)
        delete tracker.second;
}

void StreamClient::receiveLoop() {
    while (threadRunning) {
        size_t received = udpClient.receive(receiveBuffer, MAX_PACKET_LEN);
        Buffer buffer(receiveBuffer, received);

        auto packetId = buffer.get<uint8_t>();

        switch (packetId) {
            case 0x00: {
                auto *info = new TrackerInfo();
                info->trackerState.trackerId = buffer.get<uint8_t>();
                info->trackerClass = static_cast<TrackerClass>(buffer.get<uint8_t>());
                info->trackerColor = static_cast<TrackerColor>(buffer.get<uint8_t>());
                info->modelNo = buffer.getString();
                trackers[info->trackerState.trackerId] = info;
                addTrackerCallback(info);
                break;
            }
            case 0x01: {
                auto id = buffer.get<uint8_t>();
                auto *removed = trackers[id];
                trackers[id] = nullptr;
                removeTrackerCallback(removed);
                delete removed;
                break;
            }
            case 0x02: {
                auto numItems = buffer.get<uint8_t>();
                for (int i = 0; i < numItems; i++) {
                    auto trackerId = buffer.get<uint8_t>();

                    auto &state = trackers[trackerId]->trackerState;
                    state.buttons = buffer.get<uint16_t>();
                    state.position.x = buffer.get<float>();
                    state.position.y = buffer.get<float>();
                    state.position.z = buffer.get<float>();

                    state.rotation.x = buffer.get<float>();
                    state.rotation.y = buffer.get<float>();
                    state.rotation.z = buffer.get<float>();
                    state.rotation.w = buffer.get<float>();
                }

                break;
            }
            default:
                std::cerr << "Invalid packet with id " << packetId << " received." << std::endl;
                break;
        }
    }
}

void StreamClient::close() {
    threadRunning = false;
    receiveThread->join();
}

void StreamClient::setAddTrackerCallback(const StreamClient::tracker_cb &callback) {
    addTrackerCallback = callback;
}

void StreamClient::setRemoveTrackerCallback(const StreamClient::tracker_cb &callback) {
    removeTrackerCallback = callback;
}
