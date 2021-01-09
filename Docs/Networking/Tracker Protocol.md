# Tracker Protocol

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

The button IDs are defined in the definitions file.



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
0x82   [uint8 trackerClass][uint8 colorId][string serialNo]
```

`serialNo` is formatted as `[manufacturerId][chipId]`, so for TwometerVR controllers with an ESP Chip `TVR[ESP.chipId]`.

The server must respond with `0x83 Handshake Reply`



### `0x83` Handshake Reply

```
0x83   [uint8 trackerId]
```



### `..0xBF` Reserved

This packet range is reserved for (future) internal use.

Modified trackers may use the subsequent packet ID range `0xC0..0xFF` for extended functionality.