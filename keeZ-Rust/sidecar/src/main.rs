mod argon2_handler;

use serde::{Deserialize, Serialize};
use tokio::io::{AsyncBufReadExt, AsyncWriteExt, BufReader};

// ---------------------------------------------------------------------------
// Protocol types
// ---------------------------------------------------------------------------

/// Every inbound message must have an `action` discriminant.
#[derive(Deserialize)]
#[serde(tag = "action")]
enum SidecarRequest {
    #[serde(rename = "ping")]
    Ping { id: u64 },

    #[serde(rename = "argon2")]
    Argon2(argon2_handler::Argon2Request),
}

#[derive(Serialize)]
pub struct SidecarResponse {
    pub id: u64,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub result: Option<serde_json::Value>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub error: Option<String>,
}

// ---------------------------------------------------------------------------
// Entry point
// ---------------------------------------------------------------------------

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    env_logger::init();

    let stdin = tokio::io::stdin();
    let stdout = tokio::io::stdout();
    let mut reader = BufReader::new(stdin);
    let mut writer = tokio::io::BufWriter::new(stdout);
    let mut line = String::new();

    log::info!("keez-sidecar started, waiting for messages on stdin");

    loop {
        line.clear();
        let bytes_read = reader.read_line(&mut line).await?;
        if bytes_read == 0 {
            // EOF – parent process closed its end of the pipe
            log::info!("stdin EOF, exiting");
            break;
        }

        let trimmed = line.trim();
        if trimmed.is_empty() {
            continue;
        }

        let response = match serde_json::from_str::<SidecarRequest>(trimmed) {
            Ok(request) => handle_request(request).await,
            Err(e) => SidecarResponse {
                id: 0,
                result: None,
                error: Some(format!("parse error: {e}")),
            },
        };

        let response_json = serde_json::to_string(&response)?;
        writer.write_all(response_json.as_bytes()).await?;
        writer.write_all(b"\n").await?;
        writer.flush().await?;
    }

    Ok(())
}

// ---------------------------------------------------------------------------
// Request dispatch
// ---------------------------------------------------------------------------

async fn handle_request(request: SidecarRequest) -> SidecarResponse {
    match request {
        SidecarRequest::Ping { id } => SidecarResponse {
            id,
            result: Some(serde_json::Value::String("pong".to_string())),
            error: None,
        },
        SidecarRequest::Argon2(req) => argon2_handler::handle(req).await,
    }
}
