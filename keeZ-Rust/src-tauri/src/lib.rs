mod commands;
mod tray;
mod shortcuts;
mod sidecar;

use tauri::{Manager, Emitter};

pub fn run() {
    env_logger::init();

    tauri::Builder::default()
        .plugin(tauri_plugin_single_instance::init(|app, _args, _cwd| {
            // Focus existing window when a second instance is launched
            if let Some(window) = app.get_webview_window("main") {
                let _ = window.show();
                let _ = window.set_focus();
            }
        }))
        .plugin(tauri_plugin_window_state::Builder::default().build())
        .plugin(tauri_plugin_shell::init())
        .plugin(tauri_plugin_dialog::init())
        .plugin(tauri_plugin_fs::init())
        .plugin(tauri_plugin_global_shortcut::Builder::default().build())
        .plugin(tauri_plugin_notification::init())
        .plugin(tauri_plugin_process::init())
        .plugin(tauri_plugin_updater::Builder::default().build())
        .invoke_handler(tauri::generate_handler![
            commands::settings::get_settings,
            commands::settings::save_settings,
            commands::settings::delete_settings,
            commands::crypto::hardware_encrypt,
            commands::crypto::hardware_decrypt,
            commands::crypto::hardware_crypto_delete_key,
            commands::file_ops::open_file_dialog,
            commands::file_ops::save_file_dialog,
            commands::file_ops::read_file,
            commands::file_ops::write_file,
            commands::file_ops::file_exists,
            commands::process_spawn::spawn_process,
            commands::browser_extension::start_browser_extension_connector,
            commands::browser_extension::stop_browser_extension_connector,
            shortcuts::register_global_shortcut,
            shortcuts::unregister_global_shortcut,
            tray::update_tray_menu,
        ])
        .setup(|app| {
            tray::setup_tray(app)?;

            // Handle open-with for .kdbx files (files passed as CLI arguments)
            let args: Vec<String> = std::env::args().collect();
            for arg in args.iter().skip(1) {
                if arg.ends_with(".kdbx") && std::path::Path::new(arg).exists() {
                    if let Some(window) = app.get_webview_window("main") {
                        let _ = window.emit("open-file", arg);
                    }
                }
            }

            Ok(())
        })
        .on_window_event(|window, event| {
            match event {
                tauri::WindowEvent::Focused(focused) => {
                    let _ = window.emit("window-focus-changed", focused);
                }
                tauri::WindowEvent::Resized(_) | tauri::WindowEvent::Moved(_) => {}
                tauri::WindowEvent::CloseRequested { api, .. } => {
                    // Hide to tray instead of closing if configured
                    // The frontend handles this logic via the TitleBar component
                    // For system close button, we just allow it unless frontend intercepts
                    let _ = api; // suppress unused warning
                }
                _ => {}
            }
        })
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
