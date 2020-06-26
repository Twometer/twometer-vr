# Service
The Service is the core of TwometerVR. It is the bridge between all the connected devices and SteamVR, provides a user interface and handles visual controller tracking using the webcam.

> This is the rewrite for the service in order to make it more user-friendly and versatile.

## Project structure
- Core libraries
  - `TVR.Service.Core`: Contains all the tracking and processing logic
  - `TVR.Service.Network`: Implements the servers, discovery and protocols
  - `TVR.Service.Common`: Common API which makes it easier to build a frontend
- Frontends
  - `TVR.Service.CLI`: Console interface for debugging.
  - `TVR.Service.UI`: User-friendly modern GUI frontend. Includes setup assistant.

## Getting Started
For details on how to configure and run the service, check out the [Wiki](https://github.com/Twometer/twometer-vr/wiki)