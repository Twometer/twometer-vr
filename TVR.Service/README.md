# TwometerVR Service
The Service is the core of TwometerVR. It is the bridge between all the connected
devices, SteamVR and much more.

## Project organization
- Core libraries
  - `TVR.Service.Core`: In this project, the tracking logic is implemented.
  - `TVR.Service.Network`: Here, the servers and packets are implemented
  - `TVR.Service.Common`: This contains all the classes required to implement a frontend
- Frontends
  - `TVR.Service.DebugUI`: Heavyweight OpenGL-based debugging UI. It's slow, don't use for production.
  - `TVR.Service.CLI`: Light and fast CLI. Doubleclick and start playing.
  - `TVR.Service.UI`: Next-gen UI frontend: Fast, easy to set up, easy to configure. (WIP)

## Tasks
Any running instance on any frontend for the service handles the following tasks:

 - Tracking the controllers in 3D space
 - Manage button input and calibration
 - Host the orientation server for the controllers
 - Host the server for the driver

## Getting Started
For details on how to configure and run the service, check out the [Wiki](https://github.com/Twometer/twometer-vr/wiki)