using PasswordManager.Services.Interfaces.Encryption;
using System.Security.Cryptography;

namespace PasswordManager.Services.Services.Encryption
{
    public class SaltService : ISaltService
    {
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
