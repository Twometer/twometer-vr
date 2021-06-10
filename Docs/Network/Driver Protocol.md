# Driver Protocol

This UDP binary protocol is used for IPC between the server and the SteamVR driver.

### Base structure

```
[uint8 id][payload]
```



### `0x00` Tracker Connect

Sent by the server to inform the driver of a new tracker

```
[uint8 trackerId][uint8 trackerClass][uint8 trackerColor][string serialNo]
```



### `0x01` Tracker Disconnect

If a tracker stops sending status updates for longer than 30 seconds, it is considered offline and the server will inform the driver of a lost tracker using this message.

```
[uint8 trackerId]
```



### `0x02` Tracker States

Used to tell the driver about state updates

```
[TrackerStates[] states]
```




#### Tracker state

```
[uint8 trackerId][uint16 buttons][vec3 position][vec4 rotation][bool inRange]
```



### `0x03` Request Info

Sent by the driver on starting up to request tracker information.

Has no content.



### `..0x7F` Reserved

This range is reserved for future use. Extensions may use the `0x80..0xFF` packet range.