//
// Created by Twometer on 4 Jan 2021.
//

#include <iostream>
#include "Net/StreamClient.h"

int main() {
    StreamClient streamClient;

    streamClient.SetAddTrackerCallback([](TrackerInfo *tracker) {
        std::cout << "New tracker" << std::endl;
    });

    streamClient.SetRemoveTrackerCallback([](TrackerInfo *tracker) {
        std::cout << "Tracker lost" << std::endl;
    });
    for (;;);
}