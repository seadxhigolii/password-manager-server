using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordManager.Core.Domain;
using PasswordManager.Persistence.Configuration.Shared;

namespace PasswordManager.Persistence.Configuration
{
    public class UserConfiguration : BaseEntityConfiguration<User, Guid>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.PasswordEntries)
                   .WithOne(p => p.User)
                   .HasForeignKey(p => p.UserId);
        }
    }
}
