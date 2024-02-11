using System.Security.Cryptography;
using System.Text;

namespace MazeEscape.Encoder.Helper
{
    internal class AesEncryption
    {
        public static byte[] Encrypt(string plaintext, byte[] key, byte[] iv)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                var enc = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;

                using (var stream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(stream, enc, CryptoStreamMode.Write))
                    {
                        var plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = stream.ToArray();
                }
                return encryptedBytes;
            }
        }
        public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                var dec = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;

                using (var stream = new MemoryStream(ciphertext))
                {
                    using (var cryptoStream = new CryptoStream(stream, dec, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new MemoryStream())
                        {
                            cryptoStream.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
