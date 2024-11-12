using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PasswordManager.Services.Interfaces.Decryption;
using System.Security.Cryptography;

namespace PasswordManager.Services.Services.Decryption
{
    public class DecryptionService : IDecryptionService
    {
        public byte[] DeriveKeyFromPassword(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 32);
        }

        public byte[] DecryptWithAES(byte[] encryptedData, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;

                var iv = encryptedData.Take(16).ToArray();
                var actualEncryptedData = encryptedData.Skip(16).ToArray();

                aes.IV = iv;

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                return decryptor.TransformFinalBlock(actualEncryptedData, 0, actualEncryptedData.Length);
            }
        }
    }
}
