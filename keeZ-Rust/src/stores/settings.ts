import { defineStore } from 'pinia'
import { ref } from 'vue'
import { invoke } from '@tauri-apps/api/core'

export interface AppSettings {
  theme: 'dark' | 'light' | 'auto'
  locale: string
  fontSize: number
  lockOnIdle: boolean
  idleTimeoutMinutes: number
  lockOnMinimize: boolean
  lockOnFocusLoss: boolean
  clipboardClearSeconds: number
  globalShortcutAutoType: string
  globalShortcutCopyPassword: string
  globalShortcutCopyUser: string
  globalShortcutCopyUrl: string
  minimizeToTray: boolean
  showTrayIcon: boolean
  checkForUpdates: boolean
}

const DEFAULT_SETTINGS: AppSettings = {
  theme: 'dark',
  locale: 'en',
  fontSize: 14,
  lockOnIdle: true,
  idleTimeoutMinutes: 15,
  lockOnMinimize: false,
  lockOnFocusLoss: false,
  clipboardClearSeconds: 12,
  globalShortcutAutoType: 'Shift+Alt+T',
  globalShortcutCopyPassword: 'Shift+Alt+C',
  globalShortcutCopyUser: 'Shift+Alt+B',
  globalShortcutCopyUrl: 'Shift+Alt+U',
  minimizeToTray: true,
  showTrayIcon: true,
  checkForUpdates: true,
}

export const useSettingsStore = defineStore('settings', () => {
  const settings = ref<AppSettings>({ ...DEFAULT_SETTINGS })
  const loaded = ref(false)

  /** Load settings from the encrypted store on disk. */
  async function load() {
    try {
      const raw = await invoke<string | null>('get_settings', { path: 'settings.json' })
      if (raw) {
        const parsed = JSON.parse(raw) as Partial<AppSettings>
        settings.value = { ...DEFAULT_SETTINGS, ...parsed }
      }
    } catch (e) {
      console.warn('Failed to load settings:', e)
    }
    loaded.value = true
  }

  /** Persist current settings to the encrypted store on disk. */
  async function save() {
    try {
      await invoke('save_settings', {
        path: 'settings.json',
        data: JSON.stringify(settings.value),
      })
    } catch (e) {
      console.error('Failed to save settings:', e)
    }
  }

  /** Merge a partial update into the current settings and persist. */
  function update(partial: Partial<AppSettings>) {
    settings.value = { ...settings.value, ...partial }
    save()
  }

  return { settings, loaded, load, save, update }
})
