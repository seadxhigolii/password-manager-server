using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordManager.Core.Domain;
using PasswordManager.Persistence.Configuration.Shared;

namespace PasswordManager.Persistence.Configuration
{
    public class PasswordEntryConfiguration : BaseEntityConfiguration<PasswordEntry, Guid>
    {
        public void Configure(EntityTypeBuilder<PasswordEntry> builder)
        {
        }
    }
}
