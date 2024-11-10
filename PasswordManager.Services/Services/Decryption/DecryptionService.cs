using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PasswordManager.Services.Interfaces.Decryption;

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
    }
}
