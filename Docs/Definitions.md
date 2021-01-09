## Steam VR

- Universe ID: `0x02 0x16 0x39 0xA0` = `2688095746`



## Networking ports

- Server ⇄ Driver: `20156`
- Server ⇄ Tracker (Broadcast): `20157`
- Server ⇄ Tracker (Unicast): `20158`



## Protocol definitions

- All multi-byte integer values are little-endian
- Packets longer than 4 KB (4096 bytes) are not accepted by any endpoint
- `vec3`: 3 sequential 32-bit floating-point values in order `[x, y, z]` that encode a 3D-position
- `vec4`: 4 sequential 32-bit floating-point values in order `[x, y, z, w]` that encode a quaternion
- `type[]`: Array of objects, is prefixed by a `[uint8 length]` that contains the number of objects in the array.
- `string`: NUL-terminated ASCII-encoded string



## Button IDs

- `0`: A
- `1`: B
- `2`: Up
- `3`: Left
- `4`: Right
- `5`: Down
- `6`: Menu
- `7`: Trigger
- `8..15`: reserved