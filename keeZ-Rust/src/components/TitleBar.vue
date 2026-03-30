<script setup lang="ts">
import { getCurrentWindow } from '@tauri-apps/api/window'
import { useSettingsStore } from '../stores/settings'

const settingsStore = useSettingsStore()
const appWindow = getCurrentWindow()

async function minimize() {
  await appWindow.minimize()
}

async function maximize() {
  if (await appWindow.isMaximized()) {
    await appWindow.unmaximize()
  } else {
    await appWindow.maximize()
  }
}

async function close() {
  if (settingsStore.settings.minimizeToTray) {
    await appWindow.hide()
  } else {
    await appWindow.close()
  }
}
</script>

<template>
  <div class="titlebar" data-tauri-drag-region>
    <div class="titlebar-title" data-tauri-drag-region>KeeZ</div>
    <div class="titlebar-controls">
      <button class="titlebar-btn" title="Minimize" @click="minimize">&#x2014;</button>
      <button class="titlebar-btn" title="Maximize" @click="maximize">&#x25A1;</button>
      <button class="titlebar-btn titlebar-btn--close" title="Close" @click="close">&#x2715;</button>
    </div>
  </div>
</template>

<style scoped>
.titlebar {
  height: 32px;
  background: var(--titlebar-bg, #0f0f23);
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 8px;
  user-select: none;
  flex-shrink: 0;
  /* Allow Tauri to use this element for window dragging */
}

.titlebar-title {
  font-size: 13px;
  font-weight: 600;
  color: var(--text-muted, #888);
  padding-left: 8px;
}

.titlebar-controls {
  display: flex;
  gap: 4px;
}

.titlebar-btn {
  background: none;
  border: none;
  color: var(--text-muted, #888);
  width: 28px;
  height: 22px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.15s, color 0.15s;
}

.titlebar-btn:hover {
  background: rgba(255, 255, 255, 0.1);
  color: var(--text-color, #e0e0e0);
}

.titlebar-btn--close:hover {
  background: #c0392b;
  color: #fff;
}
</style>
