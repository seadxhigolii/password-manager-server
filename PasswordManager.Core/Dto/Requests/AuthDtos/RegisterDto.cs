namespace PasswordManager.Core.Dto.Requests.Auth
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string MasterPassword { get; set; }
        public string MasterPasswordRepeated { get; set; }
    }
}
