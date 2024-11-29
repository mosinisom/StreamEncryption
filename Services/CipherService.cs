using System;
using System.Text;
using System.Security.Cryptography;

namespace Backend.Services
{
  public class CipherService
  {
    public string GenerateKey()
    {
      var key = new byte[16];
      RandomNumberGenerator.Fill(key);
      return Convert.ToBase64String(key);
    }

    private byte[] GenerateGamma(string key, int length)
    {
      byte[] keyBytes = Convert.FromBase64String(key);
      byte[] gamma = new byte[length];

      uint seed = BitConverter.ToUInt32(keyBytes, 0);
      for (int i = 0; i < length; i++)
      {
        seed = 1664525 * seed + 1013904223;
        gamma[i] = (byte)(seed >> 24);
      }

      return gamma;
    }

    public string Encrypt(string text, string key)
    {
      byte[] textBytes = Encoding.UTF8.GetBytes(text);
      byte[] gamma = GenerateGamma(key, textBytes.Length);

      byte[] encrypted = new byte[textBytes.Length];
      for (int i = 0; i < textBytes.Length; i++)
      {
        encrypted[i] = (byte)(textBytes[i] ^ gamma[i]);
      }

      return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string text, string key)
    {
      byte[] encrypted = Convert.FromBase64String(text);
      byte[] gamma = GenerateGamma(key, encrypted.Length);

      byte[] decrypted = new byte[encrypted.Length];
      for (int i = 0; i < encrypted.Length; i++)
      {
        decrypted[i] = (byte)(encrypted[i] ^ gamma[i]);
      }

      return Encoding.UTF8.GetString(decrypted);
    }
  }
}