using PasswordManager.Core.Domain.Shared;

namespace PasswordManager.Core.Domain
{
    public class User : BaseEntity<Guid>
    {
        #region Properties
        public string Username { get; set; }
        public string MasterPassword { get; set; }
        public string? MasterPasswordHint { get; set; }
        public string PublicKey { get; set; }
        public string EncryptedPrivateKey { get; set; }
        public string EncryptedAESKey { get; set; }
        public string Salt { get; set; }

        #endregion Properties
        public ICollection<Vault> Vaults { get; set; }
    }
}
