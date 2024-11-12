namespace PasswordManager.Services.Interfaces.Decryption
{
    public interface IDecryptionService
    {
        byte[] DeriveKeyFromPassword(string password, byte[] salt);
        byte[] DecryptWithAES(byte[] encryptedData, byte[] key);
    }
}
