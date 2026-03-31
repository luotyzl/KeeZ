use tauri::command;

/// Encrypt arbitrary bytes using a keychain-stored AES-256-CBC key.
/// On every platform the key is stored in the OS credential store (keychain/credential
/// manager/secret service).  The IV is prepended to the returned ciphertext.
#[command]
pub fn hardware_encrypt(value: Vec<u8>) -> Result<Vec<u8>, String> {
    encrypt_with_keyring_key(&value).map_err(|e| e.to_string())
}

/// Decrypt bytes that were encrypted by `hardware_encrypt`.
/// `touch_id_prompt` is accepted for API-compatibility with the macOS Secure-Enclave
/// path but is ignored on non-macOS platforms.
#[command]
pub fn hardware_decrypt(
    value: Vec<u8>,
    touch_id_prompt: Option<String>,
) -> Result<Vec<u8>, String> {
    let _ = touch_id_prompt; // used on macOS only
    decrypt_with_keyring_key(&value).map_err(|e| e.to_string())
}

/// Delete the hardware-crypto key from the OS credential store.
#[command]
pub fn hardware_crypto_delete_key() -> Result<(), String> {
    use keyring::Entry;
    let entry = Entry::new("keez", "hardware-crypto-key").map_err(|e| e.to_string())?;
    entry.delete_password().map_err(|e| e.to_string())?;
    Ok(())
}

// ---------------------------------------------------------------------------
// Internal helpers
// ---------------------------------------------------------------------------

fn get_or_create_hardware_key() -> anyhow::Result<Vec<u8>> {
    use keyring::Entry;
    use rand::Rng;

    let entry = Entry::new("keez", "hardware-crypto-key")?;
    match entry.get_password() {
        Ok(k) => Ok(hex::decode(k)?),
        Err(_) => {
            let k: Vec<u8> = rand::thread_rng().gen::<[u8; 32]>().to_vec();
            entry.set_password(&hex::encode(&k))?;
            Ok(k)
        }
    }
}

fn encrypt_with_keyring_key(data: &[u8]) -> anyhow::Result<Vec<u8>> {
    use aes::Aes256;
    use cbc::Encryptor;
    use cbc::cipher::{BlockEncryptMut, KeyIvInit, block_padding::Pkcs7};
    use rand::Rng;

    type Aes256CbcEnc = Encryptor<Aes256>;

    let key = get_or_create_hardware_key()?;
    let iv: [u8; 16] = rand::thread_rng().gen();

    let cipher = Aes256CbcEnc::new_from_slices(&key, &iv)?;
    let encrypted = cipher.encrypt_padded_vec_mut::<Pkcs7>(data);

    let mut result = iv.to_vec();
    result.extend_from_slice(&encrypted);
    Ok(result)
}

fn decrypt_with_keyring_key(data: &[u8]) -> anyhow::Result<Vec<u8>> {
    use aes::Aes256;
    use cbc::Decryptor;
    use cbc::cipher::{BlockDecryptMut, KeyIvInit, block_padding::Pkcs7};

    type Aes256CbcDec = Decryptor<Aes256>;

    if data.len() < 16 {
        anyhow::bail!("Invalid encrypted data: too short");
    }

    let key = get_or_create_hardware_key()?;
    let (iv, ciphertext) = data.split_at(16);

    let cipher = Aes256CbcDec::new_from_slices(&key, iv)?;
    cipher.decrypt_padded_vec_mut::<Pkcs7>(ciphertext)
        .map_err(|e| anyhow::anyhow!("AES decrypt error: {:?}", e))
}
