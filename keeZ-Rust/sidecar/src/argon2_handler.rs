use argon2::{Argon2 as Argon2Hasher, Algorithm, Version, Params};
use base64::{Engine as _, engine::general_purpose::STANDARD as BASE64};
use serde::{Deserialize, Serialize};

/// Inbound Argon2 request (matches the protocol used by KeeWeb's
/// native-module-host for Argon2 key derivation).
#[derive(Deserialize)]
pub struct Argon2Request {
    pub id: u64,
    /// Base64-encoded password bytes
    pub password: String,
    /// Base64-encoded salt bytes
    pub salt: String,
    pub memory: u32,
    pub iterations: u32,
    pub parallelism: u32,
    pub hash_length: u32,
    /// 0 = Argon2d, 1 = Argon2i, 2 = Argon2id
    #[serde(rename = "type")]
    pub argon2_type: u8,
    /// Argon2 version: 0x10 (16) or 0x13 (19)
    pub version: u32,
}

#[derive(Serialize)]
pub struct SidecarResponse {
    pub id: u64,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub result: Option<serde_json::Value>,
    #[serde(skip_serializing_if = "Option::is_none")]
    pub error: Option<String>,
}

pub async fn handle(req: Argon2Request) -> crate::SidecarResponse {
    // Decode inputs
    let password = match BASE64.decode(&req.password) {
        Ok(p) => p,
        Err(e) => {
            return crate::SidecarResponse {
                id: req.id,
                result: None,
                error: Some(format!("base64 decode (password): {e}")),
            }
        }
    };

    let salt = match BASE64.decode(&req.salt) {
        Ok(s) => s,
        Err(e) => {
            return crate::SidecarResponse {
                id: req.id,
                result: None,
                error: Some(format!("base64 decode (salt): {e}")),
            }
        }
    };

    // Select algorithm variant
    let algorithm = match req.argon2_type {
        0 => Algorithm::Argon2d,
        1 => Algorithm::Argon2i,
        _ => Algorithm::Argon2id,
    };

    // Select version
    let version = if req.version == 0x10 {
        Version::V0x10
    } else {
        Version::V0x13
    };

    // Build params
    let params = match Params::new(
        req.memory,
        req.iterations,
        req.parallelism,
        Some(req.hash_length as usize),
    ) {
        Ok(p) => p,
        Err(e) => {
            return crate::SidecarResponse {
                id: req.id,
                result: None,
                error: Some(format!("argon2 params: {e}")),
            }
        }
    };

    // Hash – run on a blocking thread so we don't stall the async runtime
    let hash_length = req.hash_length as usize;
    let id = req.id;
    let result = tokio::task::spawn_blocking(move || {
        let argon2 = Argon2Hasher::new(algorithm, version, params);
        let mut output = vec![0u8; hash_length];
        argon2
            .hash_password_into(&password, &salt, &mut output)
            .map(|()| output)
    })
    .await;

    match result {
        Ok(Ok(hash)) => crate::SidecarResponse {
            id,
            result: Some(serde_json::Value::String(BASE64.encode(&hash))),
            error: None,
        },
        Ok(Err(e)) => crate::SidecarResponse {
            id,
            result: None,
            error: Some(e.to_string()),
        },
        Err(e) => crate::SidecarResponse {
            id,
            result: None,
            error: Some(format!("task panicked: {e}")),
        },
    }
}
