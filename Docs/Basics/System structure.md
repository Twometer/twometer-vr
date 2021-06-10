# System structure

TwometerVR is a complex system with many components that need to work together in order to make it work. The components described in this document are named after the naming scheme [described here](/Meta/Naming). You may want to read this first for a clearer understanding.

## Trackers

Each tracker at least requires a glowing colored 4cm sphere for positional tracking. Trackers that also report orientation, as well as hand controllers, need more intelligent logic. They are based around an ESP8266 WiFi chip. Communication with the host computer is done using WiFi and a binary UDP protocol, which is documented [here](/Network/Driver%20Protocol). 

The WiFi chip streams the orientation of the controller along with the state of any buttons to the server. It can also receive messages, so that in the future one can add communication from the server to the client. This can be useful for features like haptic feedback.

To learn more about how the server knows which trackers are which, check out the [Tracker identification page](/Basics/Tracker identification).

## Server

The server is a program running on the host computer. It has multiple different functions:

It uses the computer's webcam (preferrably a PSMove camera for high refresh rates) to track the trackers' glowing spheres. Their 3D position is then computed and stored. The server also provides the UDP endpoint to which the trackers send their orientation and button data.

The server then merges all this data and transmits it to the SteamVR driver, which is required in order to play VR games and apps with the system.

## Driver

In order to let games know about the state of the trackers, TVR implements a SteamVR extension, called a driver. This  is the last step for the tracker data, which is received from the server and converted into a format SteamVR understands.
