using PasswordManager.Core.Domain.Shared;

namespace PasswordManager.Core.Domain
{
    public class User : BaseEntity<Guid>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public IList<PasswordEntry> PasswordEntries { get; set; }
    }
}
