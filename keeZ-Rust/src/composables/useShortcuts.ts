import { invoke } from '@tauri-apps/api/core'
import { onUnmounted } from 'vue'

/**
 * Register a global OS-level keyboard shortcut that emits `eventName` via
 * the Tauri backend when pressed.  The shortcut is automatically unregistered
 * when the component that called this composable is unmounted.
 *
 * @param shortcut  Accelerator string, e.g. `"Shift+Alt+T"`
 * @param eventName Tauri event name emitted to the main window on keypress
 */
export function useGlobalShortcut(shortcut: string, eventName: string): void {
  invoke('register_global_shortcut', { shortcut, eventName }).catch((e) =>
    console.warn(`Failed to register shortcut "${shortcut}":`, e),
  )

  onUnmounted(() => {
    invoke('unregister_global_shortcut', { shortcut }).catch((e) =>
      console.warn(`Failed to unregister shortcut "${shortcut}":`, e),
    )
  })
}
