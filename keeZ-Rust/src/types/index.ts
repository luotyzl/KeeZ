// ---------------------------------------------------------------------------
// Tauri IPC types
// ---------------------------------------------------------------------------

/** Config passed to the `spawn_process` Tauri command. */
export interface TauriSpawnConfig {
  cmd: string
  args: string[]
  stdinData?: string
  throwOnStderr?: boolean
}

/** Result returned by the `spawn_process` Tauri command. */
export interface TauriSpawnResult {
  stdout: string
  stderr: string
  code: number
}

// ---------------------------------------------------------------------------
// Sidecar protocol types
// ---------------------------------------------------------------------------

/** Base shape of every request sent to the keez-sidecar over stdin. */
export interface SidecarRequest {
  action: string
  id: number
  [key: string]: unknown
}

/** Base shape of every response received from the keez-sidecar over stdout. */
export interface SidecarResponse {
  id: number
  result?: unknown
  error?: string
}

/** Argon2 hashing request sent to the sidecar. */
export interface Argon2SidecarRequest extends SidecarRequest {
  action: 'argon2'
  /** Base64-encoded password bytes */
  password: string
  /** Base64-encoded salt bytes */
  salt: string
  memory: number
  iterations: number
  parallelism: number
  hash_length: number
  /** 0 = Argon2d, 1 = Argon2i, 2 = Argon2id */
  type: 0 | 1 | 2
  /** 0x10 or 0x13 */
  version: number
}

// ---------------------------------------------------------------------------
// Settings / shortcut types
// ---------------------------------------------------------------------------

export interface GlobalShortcutConfig {
  autoType: string
  copyPassword: string
  copyUser: string
  copyUrl: string
  copyOtp: string
}
