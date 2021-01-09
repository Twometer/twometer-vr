# Tracker identification

When registering with the server, the tracker transmits its class, color and serial number. The server then generates a unique 8-bit ID for that tracker. For subsequent pose updates, only this ID is sent.

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