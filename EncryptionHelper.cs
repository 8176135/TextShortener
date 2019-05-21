using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;

namespace testingWebApp
{
    public static class EncryptionHelper
    {
        const int HashSize = 20, HashIter = 100;

        static readonly byte[] HashPepper = {0xf5, 0x0a, 0x34, 0x1b, 0x83, 0xc8, 0xdc, 0xf5, 0x27, 0x65, 0x6c, 0x87, 0x8b, 0x85, 0xac, 0xce};
        // Not much point having a random salt for encryption even if we can, since they just need to crack the searchId hash to get the password.
        static readonly byte[] PassHashPepper = {0xe7, 0x9f, 0xb6, 0x2c, 0xe4, 0x56, 0x41, 0x5a, 0x0d, 0xcc, 0x12, 0xe0, 0x40, 0x1c, 0x62, 0xf6};

        public static byte[] GenerateHash(string searchId)
        {
            return new Rfc2898DeriveBytes(searchId, HashPepper, HashIter, HashAlgorithmName.SHA256).GetBytes(HashSize);
        }


        const int Keysize = 256;

        public static byte[] Encrypt(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, PassHashPepper, HashIter))
            {
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 128;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    // IV can be constant, since each key is only used once.
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, PassHashPepper))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.

                                byte[] cipherTextBytes = memoryStream.ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return cipherTextBytes;
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(byte[] cipherTextBytes, string passPhrase)
        {
            using (var password = new Rfc2898DeriveBytes(passPhrase, PassHashPepper, HashIter))
            {
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 128;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, PassHashPepper))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }

            return randomBytes;
        }


//        public bool Verify(string password)
//        {
//            byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
//            for (int i = 0; i < HashSize; i++)
//                if (test[i] != _hash[i])
//                    return false;
//            return true;
//        }
    }
}