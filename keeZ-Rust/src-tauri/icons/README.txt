Place the following icon files in this directory before building:

  32x32.png          - 32x32 PNG icon
  128x128.png        - 128x128 PNG icon
  128x128@2x.png     - 256x256 PNG icon (2x retina)
  icon.icns          - macOS icon bundle
  icon.ico           - Windows icon
  icon.png           - System tray icon (used as trayIcon.iconPath in tauri.conf.json)

You can generate these from a single high-resolution source image using the
Tauri CLI:

  npx tauri icon path/to/source-icon.png

The source image should be at least 1024x1024 pixels with a transparent
background.
