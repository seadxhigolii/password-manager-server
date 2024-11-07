using PasswordManager.Core.Domain.Shared;

namespace PasswordManager.Core.Domain
{
    public class CustomField : BaseEntity<Guid>
    {
        #region Properties
        public Guid VaultId { get; set; }
        public string FieldName { get; set; }
        public string EncryptedFieldValue { get; set; }

        #endregion Properties
        public Vault Vault { get; set; }
    }
}
