using System.Security.Cryptography;

namespace PasswordManager.Services.Interfaces.Encryption
{
    public interface IEncryptionService
    {
        byte[] EncryptWithAES(byte[] data, byte[] key);
        byte[] HashPassword(string password, byte[] salt, int iterations = 100_000, int hashLength = 32);
    }
}
