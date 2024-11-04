using PasswordManager.Core.Domain.Shared;

namespace PasswordManager.Core.Domain
{
    public class Vault : BaseEntity<Guid>
    {
        #region Properties
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }
        public string EncryptedNotes { get; set; }
        public string Url { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime? LastAccessedOn { get; set; }

        #endregion Properties
        public User User { get; set; }
    }
}
