use tauri::{AppHandle, Emitter};
use tauri_plugin_global_shortcut::{GlobalShortcutExt, ShortcutState};

/// Register a global keyboard shortcut.  When the shortcut is pressed the
/// Tauri backend emits `event_name` to the main webview window so the
/// frontend can react (e.g. trigger auto-type, copy password, etc.).
#[tauri::command]
pub fn register_global_shortcut(
    app: AppHandle,
    shortcut: String,
    event_name: String,
) -> Result<(), String> {
    app.global_shortcut()
        .on_shortcut(shortcut.as_str(), move |app, _shortcut, event| {
            if event.state == ShortcutState::Pressed {
                if let Some(window) = app.get_webview_window("main") {
                    let _ = window.emit(&event_name, ());
                }
            }
        })
        .map_err(|e| e.to_string())?;
    Ok(())
}

/// Unregister a previously registered global shortcut.
#[tauri::command]
pub fn unregister_global_shortcut(app: AppHandle, shortcut: String) -> Result<(), String> {
    app.global_shortcut()
        .unregister(shortcut.as_str())
        .map_err(|e| e.to_string())?;
    Ok(())
}
