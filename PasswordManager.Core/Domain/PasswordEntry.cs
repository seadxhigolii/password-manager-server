using PasswordManager.Core.Domain.Shared;

namespace PasswordManager.Core.Domain
{
    public class PasswordEntry : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Password { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
