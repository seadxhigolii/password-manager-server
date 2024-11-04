using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordManager.Core.Domain;
using PasswordManager.Persistence.Configuration.Shared;

namespace PasswordManager.Persistence.Configuration
{
    public class PasswordEntryConfiguration : BaseEntityConfiguration<Vault, Guid>
    {
        public override void Configure(EntityTypeBuilder<Vault> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Username)
                   .HasMaxLength(100);

            builder.Property(p => p.EncryptedPassword)
                   .IsRequired();

            builder.Property(p => p.EncryptedNotes);

            builder.Property(p => p.Url)
                   .HasMaxLength(200);

            builder.HasIndex(p => p.UserId);

            builder.HasOne(p => p.User)
                   .WithMany(u => u.PasswordEntries)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
