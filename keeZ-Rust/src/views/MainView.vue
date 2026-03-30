<script setup lang="ts">
import { onMounted } from 'vue'
import { useAppStore } from '../stores/app'
import { useDatabaseStore } from '../stores/database'
import { useSettingsStore } from '../stores/settings'
import { useTauriEvent } from '../composables/useTauri'
import { useGlobalShortcut } from '../composables/useShortcuts'
import { useTraySync } from '../composables/useTray'

const appStore = useAppStore()
const dbStore = useDatabaseStore()
const settingsStore = useSettingsStore()

// Keep tray menu in sync with lock state
useTraySync()

// Register global shortcuts once settings are loaded
onMounted(() => {
  if (!settingsStore.loaded) return
  const s = settingsStore.settings
  useGlobalShortcut(s.globalShortcutAutoType, 'auto-type')
  useGlobalShortcut(s.globalShortcutCopyPassword, 'copy-password')
  useGlobalShortcut(s.globalShortcutCopyUser, 'copy-user')
  useGlobalShortcut(s.globalShortcutCopyUrl, 'copy-url')
})

// React to copy-field events forwarded from the app store
useTauriEvent('copy-field', (payload) => {
  console.log('copy-field event received:', payload)
})
</script>

<template>
  <div class="main-view">
    <!-- Locked state -->
    <div v-if="appStore.isLocked" class="locked-screen">
      <div class="lock-icon" aria-hidden="true">&#x1F512;</div>
      <h2>KeeZ is locked</h2>
      <p>Enter your master password to unlock</p>
      <button @click="appStore.unlock()">Unlock</button>
    </div>

    <!-- Unlocked state: placeholder for the KeeWeb web-app iframe/webview -->
    <div v-else class="app-content">
      <p class="placeholder">KeeZ password manager &mdash; Tauri 2 shell</p>
      <button @click="dbStore.openFileDialog()">Open Database&hellip;</button>
    </div>
  </div>
</template>

<style scoped>
.main-view {
  flex: 1;
  display: flex;
  overflow: hidden;
}

/* ---- Locked screen ---- */
.locked-screen {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 16px;
  color: var(--text-color, #e0e0e0);
}

.lock-icon {
  font-size: 48px;
  line-height: 1;
}

/* ---- Unlocked content ---- */
.app-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 16px;
  color: var(--text-color, #e0e0e0);
}

.placeholder {
  font-size: 16px;
  opacity: 0.6;
}

button {
  background: var(--accent-color, #2c5282);
  color: #fff;
  border: none;
  padding: 10px 20px;
  border-radius: 6px;
  cursor: pointer;
  font-size: 14px;
  transition: opacity 0.15s;
}

button:hover {
  opacity: 0.85;
}
</style>
