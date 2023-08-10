using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Utils
{
    public static class CustomPlayerPrefs
    {
        private const string SALT = "dontChangeThisKey";
        private const string ENCRYPTION_KEY = "dontChangeThisKey";

        // Encryption with errors
        private const bool USE_ENCRYPTION = false; // Clear player prefs on change

        private static readonly byte[] Salt = Encoding.ASCII.GetBytes(SALT);
        private static readonly byte[] Key = new Rfc2898DeriveBytes(ENCRYPTION_KEY, Salt).GetBytes(256 / 8);

        public static void SetString(string id, string value)
        {
            SetStringInternal(id, value);
        }

        public static string GetString(string id, string defaultValue = default)
        {
            return GetStringInternal(id, defaultValue);
        }

        public static void SetFloat(string id, float value)
        {
            SetStringInternal(id, value.ToString(CultureInfo.InvariantCulture));
        }

        public static float GetFloat(string id, float defaultValue = default)
        {
            return GetFloatInternal(id, defaultValue);
        }

        public static void SetBool(string id, bool value)
        {
            SetInt(id, value ? 1 : 0);
        }

        public static bool GetBool(string id, bool defaultValue = default)
        {
            var integerValue = GetIntInternal(id, defaultValue ? 1 : 0);
            return integerValue > 0;
        }

        public static void SetInt(string id, int value)
        {
            SetStringInternal(id, value.ToString(CultureInfo.InvariantCulture));
        }

        public static int GetInt(string id, int defaultValue = default)
        {
            return GetIntInternal(id, defaultValue);
        }

        private static void SetStringInternal(string id, string value)
        {
            try
            {
                SaveData(id, value);
            }
            catch (Exception e)
            {
                Logger.Error($"Save error : {e}");
            }
        }

        private static string GetStringInternal(string id, string defaultValue = default)
        {
            if (!PlayerPrefs.HasKey(id))
            {
                return defaultValue;
            }

            if (USE_ENCRYPTION)
            {
                var value = PlayerPrefs.GetString(id);
                return LoadData(value);
            }

            return PlayerPrefs.GetString(id);
        }

        private static int GetIntInternal(string id, int defaultValue = 0)
        {
            if (!PlayerPrefs.HasKey(id))
            {
                return defaultValue;
            }

            if (USE_ENCRYPTION)
            {
                var value = PlayerPrefs.GetString(id);
                var savedValue = LoadData(value);
                Logger.Log($"{id} = {savedValue}");
                return Convert.ToInt32(savedValue);
            }

            return Convert.ToInt32(PlayerPrefs.GetString(id));
        }

        private static float GetFloatInternal(string id, float defaultValue = 0)
        {
            if (!PlayerPrefs.HasKey(id))
            {
                return defaultValue;
            }

            var value = PlayerPrefs.GetString(id);
            return (float)Convert.ToDecimal(LoadData(value));
        }


        public static void SaveData(string key, string value)
        {
            if (USE_ENCRYPTION)
            {
                byte[] encryptedData = Encrypt(Encoding.UTF8.GetBytes(value));
                value = Convert.ToBase64String(encryptedData);
            }

            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static string LoadData(string key)
        {
            try
            {
                var encryptedData = PlayerPrefs.GetString(key);
                var decryptedData = Decrypt(Convert.FromBase64String(encryptedData));
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception e)
            {
                Logger.Error($"key {key} : {e}");
                throw;
            }
        }

        private static byte[] Encrypt(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] cipherText = encryptor.TransformFinalBlock(data, 0, data.Length);
                    byte[] encryptedData = new byte[iv.Length + cipherText.Length];
                    Buffer.BlockCopy(iv, 0, encryptedData, 0, iv.Length);
                    Buffer.BlockCopy(cipherText, 0, encryptedData, iv.Length, cipherText.Length);
                    return encryptedData;
                }
            }
        }

        private static byte[] Decrypt(byte[] encryptedData)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                var iv = new byte[16];
                Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                {
                    var cipherText = new byte[encryptedData.Length - iv.Length];
                    Buffer.BlockCopy(encryptedData, iv.Length, cipherText, 0, cipherText.Length);
                    return decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                }
            }
        }
    }
}