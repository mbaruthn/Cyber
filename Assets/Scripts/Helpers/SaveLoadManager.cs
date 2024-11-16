using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private static readonly string saveFilePath = Application.persistentDataPath + "/saveData.json";
    private static readonly string encryptionKey = "LetsMatchTheseee";

    // Save game data
    public static void SaveGame(SaveData data)
    {
        string jsonData = JsonUtility.ToJson(data);

        // Encrypt the JSON data
        string encryptedData = Encrypt(jsonData, encryptionKey);

        // Save to file
        File.WriteAllText(saveFilePath, encryptedData);
        Debug.Log("Game Saved to " + saveFilePath);
    }

    // Load game data
    public static SaveData LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file found!");
            return null;
        }

        // Read and decrypt the file content
        string encryptedData = File.ReadAllText(saveFilePath);
        string jsonData = Decrypt(encryptedData, encryptionKey);

        // Deserialize the JSON data
        return JsonUtility.FromJson<SaveData>(jsonData);
    }

    // Encrypt data using AES
    private static string Encrypt(string plainText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = new byte[16]; 

            using (var encryptor = aes.CreateEncryptor())
            {
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }

    // Decrypt data using AES
    private static string Decrypt(string encryptedText, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = new byte[16];

            using (var decryptor = aes.CreateDecryptor())
            {
                byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}
