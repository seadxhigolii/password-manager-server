using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Core.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services.Helpers
{
    public static class Generator
    {
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        public static byte[] GenerateAESKey()
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                return aes.Key;
            }
        }        
    }
}
