import { defineStore } from 'pinia'
import { ref } from 'vue'
import { emit } from '@tauri-apps/api/event'

export const useAppStore = defineStore('app', () => {
  const isLocked = ref(false)
  const activeEntryId = ref<string | null>(null)
  const searchQuery = ref('')

  /** Lock the application and notify the rest of the frontend. */
  function lock() {
    isLocked.value = true
    emit('app-locked', {})
  }

  /** Unlock the application (called after master-password verification). */
  function unlock() {
    isLocked.value = false
  }

  /** Forward auto-type trigger to the web-app layer. */
  async function triggerAutoType() {
    await emit('trigger-auto-type', {})
  }

  /** Forward copy-field shortcuts to the web-app layer. */
  async function copyPassword() {
    await emit('copy-field', { field: 'password' })
  }

  async function copyUsername() {
    await emit('copy-field', { field: 'username' })
  }

  async function copyUrl() {
    await emit('copy-field', { field: 'url' })
  }

  async function copyOtp() {
    await emit('copy-field', { field: 'otp' })
  }

  return {
    isLocked,
    activeEntryId,
    searchQuery,
    lock,
    unlock,
    triggerAutoType,
    copyPassword,
    copyUsername,
    copyUrl,
    copyOtp,
  }
})
