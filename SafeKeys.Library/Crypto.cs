using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace SafeKeys.Library
{
    public class Crypto : ICrypto
    {

        public string GenerateAuthHash(string plaintext)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(plaintext, hashType: HashType.SHA512, 15);
        }

        public bool VerifyAuthHash(string plaintext, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(plaintext, hash, hashType: HashType.SHA512);
        }

        public byte[] GenerateSalt(int length = 32)
        {
            byte[] bytes = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        public byte[] GenerateCrptoKey(string plainText, byte[] salt, int iterations = 32768, int length = 32)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(plainText), salt, iterations))
            {
                return deriveBytes.GetBytes(length);
            }
        }


        private byte[] AddSalt(byte[] plainTextBytes)
        {

            // Generate the salt.
            byte[] saltBytes = GenerateSalt(4, 8);

            // Allocate array which will hold salt and plain text bytes.
            byte[] plainTextBytesWithSalt = new byte[plainTextBytes.Length + saltBytes.Length];
            // First, copy salt bytes.
            Array.Copy(saltBytes, plainTextBytesWithSalt, saltBytes.Length);

            // Append plain text bytes to the salt value.
            Array.Copy(plainTextBytes, 0,
                        plainTextBytesWithSalt, saltBytes.Length,
                        plainTextBytes.Length);

            return plainTextBytesWithSalt;
        }

        /// <summary>
        /// Generates a random salt, with the first 4 bytes the length of the salt 
        /// </summary>      
        /// <returns></returns>
        private byte[] GenerateSalt(int minSaltLen, int maxSaltLen)
        {
            // If min and max salt values are the same, it should not be random.
            int saltLen = minSaltLen == maxSaltLen ? minSaltLen : GenerateRandomNumber(minSaltLen + 4, maxSaltLen + 4);


            // Allocate byte array to hold our salt.
            byte[] salt = new byte[saltLen];

            // Populate salt with cryptographically strong bytes.
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(salt);
            }

            // Split salt length (always one byte) into four two-bit pieces and
            // store these pieces in the first four bytes of the salt array.
            salt[0] = (byte)((salt[0] & 0xfc) | (saltLen & 0x03));
            salt[1] = (byte)((salt[1] & 0xf3) | (saltLen & 0x0c));
            salt[2] = (byte)((salt[2] & 0xcf) | (saltLen & 0x30));
            salt[3] = (byte)((salt[3] & 0x3f) | (saltLen & 0xc0));

            return salt;
        }

        private int GenerateRandomNumber(int minValue, int maxValue)
        {
            // We will make up an integer seed from 4 bytes of this array.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert four random bytes into a positive integer value.
            int seed = ((randomBytes[0] & 0x7f) << 24) |
                        (randomBytes[1] << 16) |
                        (randomBytes[2] << 8) |
                        (randomBytes[3]);

            // Now, this looks more like real randomization.
            var random = new Random(seed);

            // Calculate a random number.
            return random.Next(minValue, maxValue + 1);
        }

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="key">32 bit encryption key</param>
        /// <param name="iv">Pass nothing in to get new iv</param>
        /// <returns></returns>
        public string[] Encrypt(string plainText, byte[] key, string iv = null)
        {
            byte[] ivBytes = Array.Empty<byte>();

            if (iv != null)
            {
                ivBytes = Convert.FromBase64String(iv);
            }

            // Add salt at the beginning of the plain text bytes (if needed).
            byte[] plainTextBytesWithSalt = AddSalt(Encoding.UTF8.GetBytes(plainText));

            byte[] cipherTextBytes = null;

            // Let's make cryptographic operations thread-safe.
            lock (this)
            {
                using (var aes = new AesManaged())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = key;

                    if (iv != null)
                    {
                        aes.IV = ivBytes;
                    }                  

                    ICryptoTransform encryptor = aes.CreateEncryptor();

                    // Encryption will be performed using memory stream.
                    using (var memoryStream = new MemoryStream())
                    {

                        // To perform encryption, we must use the Write mode.
                        using (var cryptoStream = new CryptoStream(
                                                           memoryStream,
                                                           encryptor,
                                                            CryptoStreamMode.Write))
                        {

                            // Start encrypting data.
                            cryptoStream.Write(plainTextBytesWithSalt, 0, plainTextBytesWithSalt.Length);
                            // Finish the encryption operation.
                            cryptoStream.FlushFinalBlock();
                            // Move encrypted data from memory into a byte array.
                            cipherTextBytes = memoryStream.ToArray();
                            cryptoStream.Close();
                        }
                        memoryStream.Close();
                    }
                    // Return encrypted data.
                    return new string[] { Convert.ToBase64String(cipherTextBytes), Convert.ToBase64String(aes.IV) };
                }


            }
        }

        public string Decrypt(string cipherText, byte[] key, string iv)
        {

            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] ivBytes = Convert.FromBase64String(iv);

            int decryptedByteCount = 0;

            // Since we do not know how big decrypted value will be, use the same
            // size as cipher text. Cipher text is always longer than plain text
            // (in block cipher encryption), so we will just use the number of
            // decrypted data byte after we know how big it is.
            byte[] decryptedBytes = new byte[cipherTextBytes.Length];

            // Let's make cryptographic operations thread-safe.
            lock (this)
            {

                using (var aes = new AesManaged())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = key;
                    aes.IV = ivBytes;

                    ICryptoTransform decryptor = aes.CreateDecryptor();

                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        // To perform decryption, we must use the Read mode.
                        using (var cryptoStream = new CryptoStream(
                                                           memoryStream,
                                                           decryptor,
                                                           CryptoStreamMode.Read))
                        {

                            // Decrypting data and get the count of plain text bytes.
                            decryptedByteCount = cryptoStream.Read(decryptedBytes,
                                                                    0,
                                                                    decryptedBytes.Length);
                            cryptoStream.Close();
                        }
                        memoryStream.Close();
                    }
                }
            }

            // If we are using salt, get its length from the first 4 bytes of plain text data.

            int saltLen = (decryptedBytes[0] & 0x03) |
                        (decryptedBytes[1] & 0x0c) |
                        (decryptedBytes[2] & 0x30) |
                        (decryptedBytes[3] & 0xc0);


            // Allocate the byte array to hold the original plain text (without salt).
            byte[] plainTextBytes = new byte[decryptedByteCount - saltLen];

            // Copy original plain text discarding the salt value if needed.
            Array.Copy(decryptedBytes, saltLen, plainTextBytes,
                        0, decryptedByteCount - saltLen);

            // Return original plain text value.
            return Encoding.UTF8.GetString(plainTextBytes);
        }
    }
}
