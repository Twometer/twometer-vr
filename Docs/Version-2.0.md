
# Twometer VR 2.0
> Unfinished plan for the next generation of TVR

## Goals
- New IMU chip for drift-free VR experience
- More buttons on the controller to support more games
- Faster and more stable networking protocol
- Support for full-body tracking

## Controller colors and IDs
Reserved colors as determined to be the most distinguishable on camera. May change in the future
- `0x00`: Red (#FF0000): Left hand
- `0x01`: Blue (#0000FF): Right hand
- `0x02`: Green (#00FF00): Left leg
- `0x03`: Yellow (#FFFF00): Right leg
- `0x04`: Magenta (#FF00FF): Torso

## Networking ports
- Server ⇄ Driver: `20156`
- Server ⇄ Controller (Multicast): `20157`
- Server ⇄ Controller (Unicast): `20158`

## Protocol
UDP-based binary protocol.

Packet structure: `[uint8 id][payload]`
> Notes for the ID field: If MSB is set, it's a special message ID, else it's the controller id

### `0x00-0x7F` Controller State
```
[uint8 controllerId][uint16 buttons][float4 rotation]
```

`buttons` is a bitmask for pressed buttons: MSB->LSB is button 0->15

`float4` is a 32-bit floating point quaternion in order `x, y, z, w`

#### Buttons
- `0`: A
- `1`: B
- `2`: Up
- `3`: Down
- `4`: Left
- `5`: Right
- `6`: Trigger
- `7`: Menu
- `8..15`: reserved

### `0x80` Discovery (Broadcast)
```
0x80   0x02 0x16 0x39 0xA0
pID    Identifier
```

Identifier is a static key that identifies the Twometer VR 2.0 controllers. The server should respond with `0x81`

### `0x81` Discovery Reply
```
0x81   [string server_ip]
```
 `server_ip` contains the IP of the server in the form of `xxx.yyy.zzz.aaa`
 
### `0x81` Hello
```
0x81   [uint8 controller_id][string model_no]
```