//
// Created by Twometer on 9 Nov 2020.
//

#include <iostream>

#include "StreamClient.h"
#include "Buffer.h"
#include "../Utils/Constants.h"

StreamClient::StreamClient() {
    udpClient.Bind(NET_PORT);

    receiveBuffer = new uint8_t[MAX_PACKET_LEN];
    receiveThread = new std::thread([this] { ReceiveLoop(); });
}

StreamClient::~StreamClient() {
    delete receiveThread;
    for (auto tracker : trackers)
        delete tracker.second;
}

void StreamClient::ReceiveLoop() {
    while (threadRunning) {
        size_t received = udpClient.Receive(receiveBuffer, MAX_PACKET_LEN);
        Buffer buffer(receiveBuffer, received);

        auto packetId = buffer.Get<uint8_t>();

        switch (packetId) {
            case 0x00: {
                auto *info = new TrackerInfo();
                info->trackerState.trackerId = buffer.Get<uint8_t>();
                info->trackerClass = static_cast<TrackerClass>(buffer.Get<uint8_t>());
                info->trackerColor = static_cast<TrackerColor>(buffer.Get<uint8_t>());
                info->serialNo = buffer.GetString();
                trackers[info->trackerState.trackerId] = info;
                addTrackerCallback(info);
                break;
            }
            case 0x01: {
                auto id = buffer.Get<uint8_t>();
                auto *removed = trackers[id];
                trackers[id] = nullptr;
                removeTrackerCallback(removed);
                delete removed;
                break;
            }
            case 0x02: {
                auto numItems = buffer.Get<uint8_t>();
                for (int i = 0; i < numItems; i++) {
                    auto trackerId = buffer.Get<uint8_t>();
                    auto tracker = trackers[trackerId];
                    if (tracker == nullptr)
                        continue;

                    auto &state = tracker->trackerState;
                    state.buttons = buffer.Get<uint16_t>();
                    state.position.x = buffer.Get<float>();
                    state.position.y = buffer.Get<float>();
                    state.position.z = buffer.Get<float>();

                    state.rotation.x = buffer.Get<float>();
                    state.rotation.y = buffer.Get<float>();
                    state.rotation.z = buffer.Get<float>();
                    state.rotation.w = buffer.Get<float>();

                    state.inRange = buffer.Get<bool>();
                    updateTrackerCallback(tracker);
                }

                break;
            }
            default:
                std::cerr << "Invalid packet with id " << packetId << " received." << std::endl;
                break;
        }
    }
}

void StreamClient::Close() {
    threadRunning = false;
    receiveThread->join();
}

void StreamClient::SetAddTrackerCallback(const StreamClient::tracker_cb &callback) {
    addTrackerCallback = callback;
}

void StreamClient::SetUpdateTrackerCallback(const StreamClient::tracker_cb &callback) {
    updateTrackerCallback = callback;
}

void StreamClient::SetRemoveTrackerCallback(const StreamClient::tracker_cb &callback) {
    removeTrackerCallback = callback;
}


