import { invoke } from '@tauri-apps/api/core'
import { listen, type UnlistenFn } from '@tauri-apps/api/event'
import { onUnmounted } from 'vue'

/**
 * Subscribe to a Tauri event for the lifetime of the calling component.
 * The listener is automatically removed when the component unmounts.
 */
export function useTauriEvent(event: string, handler: (payload: unknown) => void): void {
  let unlisten: UnlistenFn | null = null

  listen(event, (e) => handler(e.payload)).then((fn) => {
    unlisten = fn
  })

  onUnmounted(() => {
    unlisten?.()
  })
}

// ---------------------------------------------------------------------------
// Hardware crypto helpers
// ---------------------------------------------------------------------------

/**
 * Encrypt bytes using the OS credential-store-backed AES key.
 * The returned array includes the IV prepended by the backend.
 */
export async function hardwareEncrypt(data: Uint8Array): Promise<Uint8Array> {
  const result = await invoke<number[]>('hardware_encrypt', { value: Array.from(data) })
  return new Uint8Array(result)
}

/**
 * Decrypt bytes that were encrypted by `hardwareEncrypt`.
 * `touchIdPrompt` is forwarded to the backend for macOS biometric auth UX.
 */
export async function hardwareDecrypt(
  data: Uint8Array,
  touchIdPrompt?: string,
): Promise<Uint8Array> {
  const result = await invoke<number[]>('hardware_decrypt', {
    value: Array.from(data),
    touchIdPrompt,
  })
  return new Uint8Array(result)
}

/** Delete the hardware-crypto key from the OS credential store. */
export async function hardwareCryptoDeleteKey(): Promise<void> {
  await invoke('hardware_crypto_delete_key')
}

// ---------------------------------------------------------------------------
// Process spawning
// ---------------------------------------------------------------------------

export interface SpawnConfig {
  cmd: string
  args: string[]
  stdinData?: string
  throwOnStderr?: boolean
}

export interface SpawnResult {
  stdout: string
  stderr: string
  code: number
}

/**
 * Spawn an external process from the backend and return its output.
 * Field names are converted to snake_case to match the Rust command signature.
 */
export async function spawnProcess(config: SpawnConfig): Promise<SpawnResult> {
  return await invoke<SpawnResult>('spawn_process', {
    config: {
      cmd: config.cmd,
      args: config.args,
      stdin_data: config.stdinData,
      throw_on_stderr: config.throwOnStderr,
    },
  })
}
