namespace PasswordManager.Core.Dto.Responses.AuthDtos
{
    public class UserRegisteredDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PublicKey { get; set; }
        public string EncryptedPrivateKey { get; set; }
        public string EncryptedAESKey { get; set; }
        public string Salt { get; set; }
    }
}
