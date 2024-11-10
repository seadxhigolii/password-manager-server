using PasswordManager.Services.Interfaces.Encryption;
using System.Security.Cryptography;

namespace PasswordManager.Services.Services.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        public byte[] EncryptWithAES(byte[] data, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();

                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                var encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);

                return aes.IV.Concat(encrypted).ToArray();
            }
        }
    }
}
