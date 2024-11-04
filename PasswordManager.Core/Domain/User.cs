using PasswordManager.Core.Domain.Shared;

namespace PasswordManager.Core.Domain
{
    public class User : BaseEntity<Guid>
    {
        #region Properties
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool TwoFactorEnabled { get; set; }

        #endregion Properties
        public ICollection<Vault> PasswordEntries { get; set; }
    }
}
