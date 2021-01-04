//
// Created by Twometer on 4 Jan 2021.
//

#include <iostream>
#include "Net/StreamClient.h"

int main() {
    StreamClient streamClient;

    streamClient.SetAddTrackerCallback([](TrackerInfo *tracker) {
        std::cout << "[TRACKER ADD] " << (int)tracker->trackerState.trackerId << ", " << tracker->serialNo << std::endl;
    });

    streamClient.SetUpdateTrackerCallback([](TrackerInfo *tracker) {
        std::cout << "[TRACKER UPDATE] " << (int)tracker->trackerState.trackerId << ", " << tracker->serialNo << std::endl;
    });

    streamClient.SetRemoveTrackerCallback([](TrackerInfo *tracker) {
        std::cout << "[TRACKER REMOVE] " << (int)tracker->trackerState.trackerId << ", " << tracker->serialNo << std::endl;
    });
    for (;;);
}