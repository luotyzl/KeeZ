use tauri::command;

/// Signal the sidecar to start the browser-extension native-messaging socket.
/// The sidecar process owns the actual socket; this command acts as the
/// coordination point between the frontend and the sidecar.
#[command]
pub async fn start_browser_extension_connector(
    _app: tauri::AppHandle,
    _config: serde_json::Value,
) -> Result<(), String> {
    // TODO: send a "start" message to the running sidecar via its stdin/stdout
    // channel once sidecar IPC is wired up.
    log::info!("start_browser_extension_connector called");
    Ok(())
}

/// Signal the sidecar to stop the browser-extension socket.
#[command]
pub async fn stop_browser_extension_connector(
    _app: tauri::AppHandle,
) -> Result<(), String> {
    log::info!("stop_browser_extension_connector called");
    Ok(())
}
