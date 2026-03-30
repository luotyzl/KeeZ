import { defineStore } from 'pinia'
import { ref } from 'vue'
import { invoke } from '@tauri-apps/api/core'
import { open, save } from '@tauri-apps/plugin-dialog'

export interface DatabaseInfo {
  path: string
  name: string
  isModified: boolean
  isLocked: boolean
}

export const useDatabaseStore = defineStore('database', () => {
  const openDatabases = ref<DatabaseInfo[]>([])
  const activeDatabase = ref<DatabaseInfo | null>(null)

  /** Show a native Open dialog filtered to .kdbx files. */
  async function openFileDialog(): Promise<string | null> {
    const selected = await open({
      multiple: false,
      filters: [{ name: 'KeePass Database', extensions: ['kdbx'] }],
    })
    return typeof selected === 'string' ? selected : null
  }

  /** Show a native Save dialog filtered to .kdbx files. */
  async function saveFileDialog(defaultName?: string): Promise<string | null> {
    const path = await save({
      defaultPath: defaultName,
      filters: [{ name: 'KeePass Database', extensions: ['kdbx'] }],
    })
    return path ?? null
  }

  /** Read raw bytes of a file via Tauri backend. */
  async function readFile(path: string): Promise<Uint8Array> {
    const bytes = await invoke<number[]>('read_file', { path })
    return new Uint8Array(bytes)
  }

  /** Write raw bytes to a file via Tauri backend. */
  async function writeFile(path: string, data: Uint8Array): Promise<void> {
    await invoke('write_file', { path, data: Array.from(data) })
  }

  /** Check whether a file exists at the given path. */
  async function fileExists(path: string): Promise<boolean> {
    return await invoke<boolean>('file_exists', { path })
  }

  return {
    openDatabases,
    activeDatabase,
    openFileDialog,
    saveFileDialog,
    readFile,
    writeFile,
    fileExists,
  }
})
