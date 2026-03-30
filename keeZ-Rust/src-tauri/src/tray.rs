use tauri::{
    menu::{Menu, MenuItem},
    tray::TrayIconBuilder,
    Manager, Runtime, Emitter,
};

/// Set up the system-tray icon with an Open / Lock / Quit context menu.
pub fn setup_tray<R: Runtime>(app: &tauri::App<R>) -> tauri::Result<()> {
    let open = MenuItem::with_id(app, "open", "Open KeeZ", true, None::<&str>)?;
    let lock = MenuItem::with_id(app, "lock", "Lock", true, None::<&str>)?;
    let quit = MenuItem::with_id(app, "quit", "Quit", true, None::<&str>)?;

    let menu = Menu::with_items(app, &[&open, &lock, &quit])?;

    TrayIconBuilder::new()
        .menu(&menu)
        .on_menu_event(|app, event| match event.id.as_ref() {
            "open" => {
                if let Some(window) = app.get_webview_window("main") {
                    let _ = window.show();
                    let _ = window.set_focus();
                }
            }
            "lock" => {
                if let Some(window) = app.get_webview_window("main") {
                    let _ = window.emit("lock-app", ());
                }
            }
            "quit" => {
                app.exit(0);
            }
            _ => {}
        })
        .on_tray_icon_event(|tray, event| {
            use tauri::tray::TrayIconEvent;
            // Single-click or double-click on the tray icon restores the window
            if let TrayIconEvent::Click { .. } = event {
                let app = tray.app_handle();
                if let Some(window) = app.get_webview_window("main") {
                    let _ = window.show();
                    let _ = window.set_focus();
                }
            }
        })
        .build(app)?;

    Ok(())
}

/// Update tray menu labels when the app lock state changes.
/// Currently a no-op placeholder; full implementation would look up the tray
/// handle via `AppHandle` and rebuild the menu with updated labels.
#[tauri::command]
pub fn update_tray_menu(_locked: bool) -> Result<(), String> {
    Ok(())
}
