namespace PasswordManager.Services.Interfaces.Encryption
{
    public interface ISaltService
    {
        byte[] GenerateSalt();
    }
}
