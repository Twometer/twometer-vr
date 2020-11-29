
# Twometer VR 2.0.0
Roadmap and design concept for the next generation of Twometer VR



## Roadmap

- New IMU chip for a drift-free VR experience
- More buttons on the controller to support more games
- Faster and more stable networking protocol
- Flexible, extensible, yet simple design
- Support multiple cameras and arbitrary angles
- Support 5P full-body tracking

## Naming

- Tracker: Any tracked device.
- Controller: Handheld tracked devices with button input capabilities.
- Server: The main server running in the TwometerVR service. May also be called 'service'.
- Driver: The 'driver' extension for SteamVR.

## Versioning

- TwometerVR uses sematic versioning, that means MAJOR.MINOR.PATCH

- The new version will be a complete rewrite/rebuild of the TVR system, therefore it's 2.0.0

- Some subprojects have wrong or unassigned version numbers. With the next TVR, every subproject will have the same MAJOR version. However, each one may have different MINOR and PATCH parts.

  TVR 2.0.0 resets everything to 2.0.0

## Tracker identification

When registering with the server, the tracker transmits its class, color and model number. The server then generates a unique 8-bit ID for that tracker. For subsequent pose updates, only this ID is sent.

### Tracker colors

Trackers are identified by the color of their tracking ball. The following colors are reserved as they were determined to be the most distinguishable by the video camera. May change in the future as tests go on.

- `0x00`: Invalid (internal use)
- `0x01`: Red (#FF0000): Left hand
- `0x02`: Blue (#0000FF): Right hand
- `0x03`: Green (#00FF00): Left leg
- `0x04`: Magenta (#FF00FF): Right leg
- `0x05`: Cyan (#FFFF00): Hips
- `0x06`: Yellow (#FFFF00): Generic
  - Not required for 5-point body tracking and may therefore be used for any other object that should be tracked.
- `..0x7F`: Reserved for future use
- `0x80..0xFF`: May be used by extensions

### Tracker classes

- `0x00`: Invalid (internal use)
- `0x01`: Controller
- `0x02`: Body tracker
- `0x03`: Generic tracker
- `..0x7F`: Reserved for future use
- `0x80..0xFF`: May be used by extensions

### Tracker IDs

Tracker IDs are a random ID that is used to identify all trackers after `0x82 Handshake`. 

To ensure uniqueness in the current tracking setup, IDs are managed by the server. The server informs the client of its ID using the `0x83 Handshake Reply` packet.

## Next-gen positional tracking

The next generation positional tracking should allow for arbitrary camera angles, and multiple cameras for a larger tracking space and more accuracy.

#### Definitions

- Image space: Raw camera image space, consisting of a 2D image position and a radius
- Camera space: Ball position in 3D space, unprojected using camera parameters, but has its origin at the camera.
- World space: Ball position in 3D space, transformed so that the origin is at the center of the playspace.

#### Basic algorithm (idea)

1. Isolate the tracker ball from the image and determine position and radius
2. Using the previously determined camera parameters, unproject the tracker ball into camera space.
3. By comparing the nominal playspace cube with the rotated playspace cube registered during a calibration step, compute a matrix from camera space into world space.
4. Use said matrix to convert all camera space ball locations into world space ball locations.

## Steam VR

- Universe ID: `0x02 0x16 0x39 0xA0` = `2688095746`

## Protocol definitions

- All multi-byte integer values are little-endian
- Packets longer than 4 KB (4096 bytes) are not accepted by any endpoint
- `vec3`: 3 sequential 32-bit floating-point values in order `[x, y, z]` that encode a 3D-position
- `vec4`: 4 sequential 32-bit floating-point values in order `[x, y, z, w]` that encode a quaternion
- `type[]`: Array of objects, is prefixed by a `[uint8 length]` that contains the number of objects in the array.
- `string`: NUL-terminated ASCII-encoded string

## Networking ports

- Server ⇄ Driver: `20156`
- Server ⇄ Tracker (Broadcast): `20157`
- Server ⇄ Tracker (Unicast): `20158`

## Driver Protocol

This UDP binary protocol is used for IPC between the server and the SteamVR driver.

### Base structure
```
[uint8 id][payload]
```



### 0x00 Tracker Connect

```
[uint8 trackerId][uint8 trackerClass][uint8 trackerColor][string modelNo]
```



### 0x01 Tracker Disconnect

If a tracker stops sending status updates for longer than 30 seconds, it is considered offline and the server will inform the driver of a lost tracker using this message.

```
[uint8 trackerId]
```



### 0x02 Tracker States

Used to tell the driver about state updates

```
[TrackerStates[] states]
```

#### Tracker state

```
[uint8 trackerId][uint16 buttons][vec3 position][vec4 rotation]
```



### `..0x7F` Reserved

This range is reserved for future use. Extensions may use the `0x80..0xFF` packet range.

## Tracker Protocol

Also an UDP-based binary protocol; used to tell the server about the tracker state.

### Base structure
```
[uint8 id][payload]
```

I the MSB of `id` is set (`id > 0x7F`), then the lower 7 bytes of ID are treated as the ID of a management packet. If it's not  set, the packet is interpreted as a tracker state packet and the lower 7 bytes are the tracker ID.



### `0x00-0x7F` Tracker State
```
[uint8 trackerId][uint16 buttons][vec4 rotation]
```

`buttons` is a bitmask for pressed buttons: LSB->MSB is button 0->15

#### Button IDs

- `0`: A
- `1`: B
- `2`: Up
- `3`: Left
- `4`: Right
- `5`: Down
- `6`: Menu
- `7`: Trigger
- `8..15`: reserved



### `0x80` Discovery (Use Broadcast Channel)

```
0x80   0x02 0x16 0x39 0xA0
pID    Identifier
```

Identifier is a static key that identifies the Twometer VR 2.0 universe. It's the same as the SteamVR universe ID.

The server must respond with `0x81 Discovery Reply`



### `0x81` Discovery Reply

```
0x81   [string serverIp]
```
 `serverIp` contains the IP or hostname of the server.



### `0x82` Handshake

```
0x82   [uint8 trackerClass][uint8 colorId][string modelNo]
```

`modelNo` can be the ESP chip's model number.

The server must respond with `0x83 Handshake Reply`



### `0x83` Handshake Reply

```
0x83   [uint8 trackerId]
```



### `..0xBF` Reserved

This packet range is reserved for (future) internal use.

Modified trackers may use the subsequent packet ID range `0xC0..0xFF` for extended functionality.