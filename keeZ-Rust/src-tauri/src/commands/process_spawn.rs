use tauri::command;
use serde::{Deserialize, Serialize};
use std::process::Command;

#[derive(Deserialize)]
pub struct SpawnConfig {
    pub cmd: String,
    pub args: Vec<String>,
    pub stdin_data: Option<String>,
    pub throw_on_stderr: Option<bool>,
}

#[derive(Serialize)]
pub struct SpawnResult {
    pub stdout: String,
    pub stderr: String,
    pub code: i32,
}

/// Spawn an external process, optionally feed it data on stdin, and return
/// its stdout/stderr/exit-code.  Mirrors the Electron `spawn-process` IPC
/// handler from the original desktop shell.
#[command]
pub fn spawn_process(config: SpawnConfig) -> Result<SpawnResult, String> {
    let mut cmd = Command::new(&config.cmd);
    cmd.args(&config.args);

    if config.stdin_data.is_some() {
        cmd.stdin(std::process::Stdio::piped());
    }
    cmd.stdout(std::process::Stdio::piped());
    cmd.stderr(std::process::Stdio::piped());

    let mut child = cmd.spawn().map_err(|e| e.to_string())?;

    if let Some(data) = &config.stdin_data {
        if let Some(stdin) = child.stdin.take() {
            use std::io::Write;
            let mut stdin = stdin;
            stdin.write_all(data.as_bytes()).map_err(|e| e.to_string())?;
            // stdin is dropped here, sending EOF to the child
        }
    }

    let output = child.wait_with_output().map_err(|e| e.to_string())?;

    let stderr = String::from_utf8_lossy(&output.stderr).to_string();
    if config.throw_on_stderr.unwrap_or(false) && !stderr.is_empty() {
        return Err(stderr);
    }

    Ok(SpawnResult {
        stdout: String::from_utf8_lossy(&output.stdout).to_string(),
        stderr,
        code: output.status.code().unwrap_or(-1),
    })
}
