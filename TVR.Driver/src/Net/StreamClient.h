//
// Created by Twometer on 9 Nov 2020.
//

#ifndef TVR_DRIVER_STREAMCLIENT_H
#define TVR_DRIVER_STREAMCLIENT_H

#include <map>
#include <thread>
#include <functional>
#include "UdpClient.h"
#include "../Model/TrackerInfo.h"

class StreamClient {
private:
    UdpClient udpClient;

    uint8_t *receiveBuffer;

    std::thread *receiveThread;

    bool threadRunning = true;

    std::map<uint8_t, TrackerInfo *> trackers;

    typedef std::function<void(TrackerInfo *)> tracker_cb;
    tracker_cb addTrackerCallback{};
    tracker_cb removeTrackerCallback{};

public:
    StreamClient();

    ~StreamClient();

    void Close();

    void SetAddTrackerCallback(const tracker_cb &callback);

    void SetRemoveTrackerCallback(const tracker_cb &callback);

private:
    void ReceiveLoop();

};


#endif //TVR_DRIVER_STREAMCLIENT_H
