import { invoke } from '@tauri-apps/api/core'
import { watch } from 'vue'
import { useAppStore } from '../stores/app'

/**
 * Keep the system-tray menu in sync with the application lock state.
 * Call once at the root component level.
 */
export function useTraySync(): void {
  const appStore = useAppStore()

  watch(
    () => appStore.isLocked,
    async (locked) => {
      try {
        await invoke('update_tray_menu', { locked })
      } catch (e) {
        console.warn('Failed to update tray menu:', e)
      }
    },
    { immediate: true },
  )
}
