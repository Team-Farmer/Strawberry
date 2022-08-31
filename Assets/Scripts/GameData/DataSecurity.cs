using System.IO;
using System.Text;
using UnityEngine;
using xxHashSharp;

public static class DataSecurity
{
    public static string GetPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, CalculateHash(key).ToString());
    }

    public static void Save(string key, string json)
    {
        var cipherText = StringCipher.Encrypt(json, key);
        File.WriteAllText(GetPath(key), cipherText);
    }

    public static string Load(string key)
    {
        string path = GetPath(key);

        if (File.Exists(path) == false)
            return string.Empty;

        var cipherText = File.ReadAllText(GetPath(key));
        return StringCipher.Decrypt(cipherText, key);
    }

    public static void DeleteLocal(string key)
    {
        string path = GetPath(key);
        if (File.Exists(path))
            File.Delete(path);
    }

    public static uint CalculateHash(string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        return xxHash.CalculateHash(bytes);
    }
}