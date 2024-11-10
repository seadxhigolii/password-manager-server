namespace PasswordManager.Services.Interfaces.Encryption
{
    public interface IEncryptionService
    {
        byte[] EncryptWithAES(byte[] data, byte[] key);
    }
}
