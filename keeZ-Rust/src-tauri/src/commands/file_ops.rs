use tauri::command;

/// Programmatic counterpart to the dialog-based open flow.
/// Returns `None` – the actual dialog is driven from the frontend via
/// `@tauri-apps/plugin-dialog`.  This command exists so the frontend can
/// also request a path open without showing a dialog (e.g. auto-open last
/// file on startup).
#[command]
pub fn open_file_dialog(_filters: Vec<(String, Vec<String>)>) -> Result<Option<String>, String> {
    // Dialog is handled by tauri-plugin-dialog from the frontend
    Ok(None)
}

/// Programmatic counterpart to the dialog-based save flow.
#[command]
pub fn save_file_dialog(_default_name: Option<String>) -> Result<Option<String>, String> {
    // Dialog is handled by tauri-plugin-dialog from the frontend
    Ok(None)
}

/// Read the raw bytes of a file at the given absolute path.
#[command]
pub fn read_file(path: String) -> Result<Vec<u8>, String> {
    std::fs::read(&path).map_err(|e| e.to_string())
}

/// Write raw bytes to a file, creating parent directories as needed.
#[command]
pub fn write_file(path: String, data: Vec<u8>) -> Result<(), String> {
    if let Some(parent) = std::path::Path::new(&path).parent() {
        std::fs::create_dir_all(parent).map_err(|e| e.to_string())?;
    }
    std::fs::write(&path, data).map_err(|e| e.to_string())
}

/// Check whether a file (or directory) exists at the given path.
#[command]
pub fn file_exists(path: String) -> bool {
    std::path::Path::new(&path).exists()
}
