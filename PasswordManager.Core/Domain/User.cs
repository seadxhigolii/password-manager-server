using PasswordManager.Core.Domain.Shared;

namespace PasswordManager.Core.Domain
{
    public class User : BaseEntity<Guid>
    {
        #region Properties
        public string Username { get; set; }
        public string Email { get; set; }
        public string MasterPassword { get; set; }
        public string MasterPasswordHint { get; set; }
        public bool TwoFactorEnabled { get; set; }

        #endregion Properties
        public ICollection<Vault> Vaults { get; set; }
    }
}
