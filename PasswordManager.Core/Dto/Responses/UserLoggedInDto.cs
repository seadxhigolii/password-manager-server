namespace PasswordManager.Core.Dto.Responses
{
    public class UserLoggedInDto
    {
        public Guid UserId { get; set; }
        public string? AuthToken { get; set; }
        public string? PrivateKey { get; set; }
        public string? PublicKey { get; set; }
        public string? Username { get; set; }
    }
}
