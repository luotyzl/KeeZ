// Sidecar management – spawning and communicating with the keez-sidecar process.
//
// The sidecar is a separate Rust binary that handles CPU-intensive operations
// (Argon2 hashing, browser-extension native-messaging socket, YubiKey, etc.)
// outside the main Tauri process.  It communicates over stdin/stdout using
// newline-delimited JSON, mirroring the pattern used by the original
// Electron native-module-host.

use tauri::AppHandle;
use tauri_plugin_shell::ShellExt;

/// Spawn the keez-sidecar binary bundled inside the Tauri app package.
/// Returns immediately; the caller should keep the `Child` handle alive for
/// the lifetime of the sidecar process.
pub async fn spawn_sidecar(app: &AppHandle) -> anyhow::Result<()> {
    let sidecar_command = app.shell().sidecar("keez-sidecar")?;
    let (_rx, _child) = sidecar_command.spawn()?;
    // TODO: store `_child` in app state so it can be communicated with and
    // killed cleanly on shutdown.
    Ok(())
}
