use tauri::command;
use keyring::Entry;
use aes::Aes256;
use cbc::{Encryptor, Decryptor};
use cbc::cipher::{BlockEncryptMut, BlockDecryptMut, KeyIvInit};
use rand::Rng;

type Aes256CbcEnc = Encryptor<Aes256>;
type Aes256CbcDec = Decryptor<Aes256>;

fn get_or_create_encryption_key() -> anyhow::Result<Vec<u8>> {
    let entry = Entry::new("keez", "settings-key")?;
    match entry.get_password() {
        Ok(key_hex) => Ok(hex::decode(key_hex)?),
        Err(_) => {
            let key: Vec<u8> = rand::thread_rng().gen::<[u8; 32]>().to_vec();
            entry.set_password(&hex::encode(&key))?;
            Ok(key)
        }
    }
}

/// Retrieve and decrypt settings stored at the given relative path under the app data dir.
/// Returns `None` if the file does not exist.
#[command]
pub fn get_settings(path: String) -> Result<Option<String>, String> {
    let file_path = dirs::data_local_dir()
        .ok_or("No data local dir")?
        .join("keez")
        .join(&path);

    if !file_path.exists() {
        return Ok(None);
    }

    let encrypted = std::fs::read(&file_path).map_err(|e| e.to_string())?;
    if encrypted.len() < 16 {
        return Ok(None);
    }

    let key = get_or_create_encryption_key().map_err(|e| e.to_string())?;
    let (iv, ciphertext) = encrypted.split_at(16);

    let decrypted = decrypt_aes256cbc(&key, iv, ciphertext).map_err(|e| e.to_string())?;
    Ok(Some(String::from_utf8(decrypted).map_err(|e| e.to_string())?))
}

/// Encrypt and persist settings to the given relative path under the app data dir.
#[command]
pub fn save_settings(path: String, data: String) -> Result<(), String> {
    let app_dir = dirs::data_local_dir()
        .ok_or("No data local dir")?
        .join("keez");

    std::fs::create_dir_all(&app_dir).map_err(|e| e.to_string())?;

    let key = get_or_create_encryption_key().map_err(|e| e.to_string())?;
    let iv: [u8; 16] = rand::thread_rng().gen();
    let encrypted = encrypt_aes256cbc(&key, &iv, data.as_bytes()).map_err(|e| e.to_string())?;

    let mut output = iv.to_vec();
    output.extend_from_slice(&encrypted);

    std::fs::write(app_dir.join(&path), output).map_err(|e| e.to_string())?;
    Ok(())
}

/// Remove a settings file.
#[command]
pub fn delete_settings(path: String) -> Result<(), String> {
    let file = dirs::data_local_dir()
        .ok_or("No data local dir")?
        .join("keez")
        .join(&path);

    if file.exists() {
        std::fs::remove_file(file).map_err(|e| e.to_string())?;
    }
    Ok(())
}

fn encrypt_aes256cbc(key: &[u8], iv: &[u8], data: &[u8]) -> anyhow::Result<Vec<u8>> {
    use cbc::cipher::block_padding::Pkcs7;
    let cipher = Aes256CbcEnc::new_from_slices(key, iv)?;
    Ok(cipher.encrypt_padded_vec_mut::<Pkcs7>(data))
}

fn decrypt_aes256cbc(key: &[u8], iv: &[u8], data: &[u8]) -> anyhow::Result<Vec<u8>> {
    use cbc::cipher::block_padding::Pkcs7;
    let cipher = Aes256CbcDec::new_from_slices(key, iv)?;
    cipher.decrypt_padded_vec_mut::<Pkcs7>(data)
        .map_err(|e| anyhow::anyhow!("AES decrypt error: {:?}", e))
}
