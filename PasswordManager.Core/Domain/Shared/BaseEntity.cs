namespace PasswordManager.Core.Domain.Shared
{
    public class BaseEntity<TId>
    {
        #region Properties
        public TId Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? ChangedOn { get; set; } = DateTime.UtcNow;
        public string? ChangedBy { get; set; }
        public bool Deleted { get; set; }
        public bool Modified { get; set; }

        #endregion Properties
    }
}
