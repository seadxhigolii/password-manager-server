using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordManager.Core.Domain;
using PasswordManager.Persistence.Configuration.Shared;

namespace PasswordManager.Persistence.Configuration
{
    public class VaultConfiguration : BaseEntityConfiguration<Vault, Guid>
    {
        public override void Configure(EntityTypeBuilder<Vault> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.ItemType)
                   .IsRequired();

            builder.Property(p => p.EncryptedNotes);

            builder.Property(p => p.IsFavorite)
                   .IsRequired();

            builder.Property(p => p.LastAccessedOn);

            builder.Property(p => p.Username)
                   .HasMaxLength(100);

            builder.Property(p => p.EncryptedPassword);

            builder.Property(p => p.Url)
                   .HasMaxLength(200);

            builder.Property(p => p.PasswordHistory);

            builder.Property(p => p.CardholderName)
                   .HasMaxLength(100);

            builder.Property(p => p.EncryptedCardNumber);

            builder.Property(p => p.ExpirationDate)
                   .HasMaxLength(5); // Format: MM/YY

            builder.Property(p => p.EncryptedSecurityCode);

            builder.HasIndex(p => p.UserId);

            builder.HasOne(p => p.User)
                   .WithMany(u => u.Vaults)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.CustomFields)
                   .WithOne(cf => cf.Vault)
                   .HasForeignKey(cf => cf.VaultId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
