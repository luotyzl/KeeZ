<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { check } from '@tauri-apps/plugin-updater'
import { useSettingsStore } from '../stores/settings'

const settingsStore = useSettingsStore()
const updateAvailable = ref(false)
const updateVersion = ref('')
const installing = ref(false)

onMounted(async () => {
  if (!settingsStore.settings.checkForUpdates) return

  try {
    const update = await check()
    if (update?.available) {
      updateAvailable.value = true
      updateVersion.value = update.version
    }
  } catch {
    // Silently ignore update check failures (offline, server unreachable, etc.)
  }
})

async function installUpdate() {
  installing.value = true
  try {
    const update = await check()
    if (update?.available) {
      await update.downloadAndInstall()
    }
  } catch (e) {
    console.error('Update install failed:', e)
    installing.value = false
  }
}

function dismiss() {
  updateAvailable.value = false
}
</script>

<template>
  <div v-if="updateAvailable" class="update-notification" role="alert">
    <span>Update available: v{{ updateVersion }}</span>
    <button :disabled="installing" @click="installUpdate">
      {{ installing ? 'Installing…' : 'Install' }}
    </button>
    <button @click="dismiss">Dismiss</button>
  </div>
</template>

<style scoped>
.update-notification {
  position: fixed;
  bottom: 16px;
  right: 16px;
  background: var(--accent-color, #2c5282);
  color: #fff;
  padding: 10px 16px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 13px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.3);
  z-index: 9999;
}

.update-notification button {
  background: rgba(255, 255, 255, 0.2);
  border: none;
  color: #fff;
  padding: 4px 10px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 12px;
  transition: background 0.15s;
}

.update-notification button:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.35);
}

.update-notification button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
