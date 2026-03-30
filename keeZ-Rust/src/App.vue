<script setup lang="ts">
import { onMounted } from 'vue'
import { listen } from '@tauri-apps/api/event'
import TitleBar from './components/TitleBar.vue'
import UpdateNotification from './components/UpdateNotification.vue'
import { useAppStore } from './stores/app'
import { useSettingsStore } from './stores/settings'

const appStore = useAppStore()
const settingsStore = useSettingsStore()

onMounted(async () => {
  // Load persisted settings before anything else renders
  await settingsStore.load()

  // Listen for events emitted by the Tauri backend (tray menu, shortcuts, etc.)
  await listen('lock-app', () => appStore.lock())
  await listen('auto-type', () => appStore.triggerAutoType())
  await listen('copy-password', () => appStore.copyPassword())
  await listen('copy-user', () => appStore.copyUsername())
  await listen('copy-url', () => appStore.copyUrl())
  await listen('copy-otp', () => appStore.copyOtp())
})
</script>

<template>
  <div class="app">
    <TitleBar />
    <router-view />
    <UpdateNotification />
  </div>
</template>

<style>
/* Global reset */
*,
*::before,
*::after {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

html,
body,
#app,
.app {
  width: 100%;
  height: 100%;
  overflow: hidden;
}

.app {
  display: flex;
  flex-direction: column;
  background: var(--bg-color, #1a1a2e);
  color: var(--text-color, #e0e0e0);
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
}
</style>
