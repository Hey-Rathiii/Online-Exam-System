using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OnlineExamSystem.Helper
{
    public static class CryptoHelper
    {
        private static readonly string EncryptionKey = "YourStrongKey1234"; // 16/24/32 chars for AES (key size matters)

        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, Encoding.ASCII.GetBytes("SaltIsHere1234"));
                aes.Key = pdb.GetBytes(32); // AES-256
                aes.IV = pdb.GetBytes(16);  // AES Block size = 128-bit

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    encrypted = ms.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decrypted;

            using (Aes aes = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, Encoding.ASCII.GetBytes("SaltIsHere1234"));
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    decrypted = ms.ToArray();
                }
            }

            return Encoding.UTF8.GetString(decrypted);
        }
    }

}