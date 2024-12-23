﻿using Microsoft.EntityFrameworkCore;
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

            builder.Property(p => p.ItemType);

            builder.Property(p => p.EncryptedNotes).IsRequired(false);

            builder.Property(p => p.IsFavorite).IsRequired(false);

            builder.Property(p => p.LastAccessedOn).IsRequired(false);

            builder.Property(p => p.Username).HasMaxLength(100).IsRequired(false);

            builder.Property(p => p.EncryptedPassword).IsRequired(false);

            builder.Property(p => p.Url).HasMaxLength(200).IsRequired(false);

            builder.Property(p => p.FavIcon).IsRequired(false);

            builder.Property(p => p.PasswordHistory).IsRequired(false); ;

            builder.Property(p => p.CardholderName).HasMaxLength(100).IsRequired(false);

            builder.Property(p => p.EncryptedCardNumber).IsRequired(false);

            builder.Property(p => p.ExpirationDate).HasMaxLength(5).IsRequired(false); // Format: MM/YY

            builder.Property(p => p.EncryptedSecurityCode).IsRequired(false);

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
