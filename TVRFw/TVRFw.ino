void setup() {
  Serial.begin(9600);     // Ah yes, debug
  Serial.println("Twometer VR Firmware v1.0");
  
  WiFi.persistent(true);  // Save those credentials
  WiFi.mode(WIFI_STA);    // Station mode for ESP-firmware glitch prevention
  WiFi.begin(WIFI_SSID, WIFI_PASS);

  Serial.println("Connecting to WiFi...");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
  }
  Serial.println("Connected");

  // Now, connect to the server
  // To find it we need some discovery...
}

void loop() {
  // Read accelerometer
  // Read buttons
  // Send it to the server
}
