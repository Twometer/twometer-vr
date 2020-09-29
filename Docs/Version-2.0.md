
# Twometer VR 2.0
Roadmap and design concept for the next generation of Twometer VR



## Roadmap
- New IMU chip for a drift-free VR experience
- More buttons on the controller to support more games
- Faster and more stable networking protocol
- Flexible, extensible, yet simple design
- Support multiple cameras and arbitrary angles
- Support 5P full-body tracking

## Controller colors and IDs
Reserved colors as determined to be the most distinguishable on video. May change in the future as tests go on
- `0x00`: Red (#FF0000): Left hand
- `0x01`: Blue (#0000FF): Right hand
- `0x02`: Green (#00FF00): Left leg
- `0x03`: Yellow (#FFFF00): Right leg
- `0x04`: Magenta (#FF00FF): Hip
- `0x05`: Cyan (#FFFF00): Extra
  - May be used for an external camera or anything else
- `0x06..0x7F`: Reserved for future use
- `0x80..0xFF`: For other, custom (user-created) controllers and colors

## Steam VR

- Universe ID: `0x02 0x16 0x39 0xA0` = `2688095746`

## Protocol definitions

- `vec3`: 3 sequential 32-bit floating-point values in order `x, y, z` that encode a 3D-position
- `quat`: 4 sequential 32-bit floating-point values in order `x, y, z, w` that encode a quaternion
- `type[]`: Array of objects, is prefixed by a `[uint8 length]` that contains the number of objects in the array.
- `string`: NUL-terminated ASCII-encoded string

## Networking ports

- Server ⇄ Driver: `20156`
- Server ⇄ Controller (Broadcast): `20157`
- Server ⇄ Controller (Unicast): `20158`

## Driver Protocol

This UDP binary protocol is used for IPC between the server and the SteamVR driver.

### Base structure
```
[uint8 id][payload]
```



### 0x00 Controller Connect

```
[uint8 controllerId][string modelNo]
```



### 0x01 Controller Disconnect

```
[uint8 controllerId]
```



### 0x02 Controller States

```
[ControllerState[] states]
```

#### Controller state

```
[uint8 controllerId][uint16 buttons][vec3 position][quat rotation]
```



## Controller Protocol

Also an UDP-based binary protocol; used to tell the server about the controller state.

### Base structure
```
[uint8 id][payload]
```

I the MSB of `id` is set (`id > 0x7F`), then the lower 7 bytes of ID are treated as the ID of a management packet. If it's not  set, the packet is interpreted as a controller state packet and the lower 7 bytes are the controller's ID.



### `0x00-0x7F` Controller State
```
[uint8 controllerId][uint16 buttons][quat rotation]
```

`buttons` is a bitmask for pressed buttons: MSB->LSB is button 0->15

#### Button IDs

- `0`: A
- `1`: B
- `2`: Up
- `3`: Down
- `4`: Left
- `5`: Right
- `6`: Trigger
- `7`: Menu
- `8..15`: reserved



### `0x80` Discovery (Use Broadcast Channel)

```
0x80   0x02 0x16 0x39 0xA0
pID    Identifier
```

Identifier is a static key that identifies the Twometer VR 2.0 controllers. It's the same as the SteamVR universe ID.

The server must respond with `0x81 Discovery Reply`



### `0x81` Discovery Reply

```
0x81   [string serverIp]
```
 `serverIp` contains the IP of the server in the form of `xxx.yyy.zzz.aaa`



### `0x82` Hello

```
0x82   [uint8 controllerId][string modelNo]
```

`modelNo` can be the ESP chip's model number.



### `0x83` Haptic Feedback (To controller only)

```
0x83   [uint16 durationMs]
```



### `0x80..0xBF` Reserved

This packet range is reserved for (future) internal use.

Modified controllers may use the subsequent packet ID range `0xC0..0xFF` for extended functionality.