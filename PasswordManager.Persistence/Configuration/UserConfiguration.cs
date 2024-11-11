using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordManager.Core.Domain;
using PasswordManager.Persistence.Configuration.Shared;

namespace PasswordManager.Persistence.Configuration
{
    public class UserConfiguration : BaseEntityConfiguration<User, Guid>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.MasterPassword)
                   .IsRequired();

            builder.Property(u => u.MasterPasswordHint)
                   .IsRequired(false); ;

            builder.Property(u => u.PublicKey);

            builder.Property(u => u.EncryptedPrivateKey);

            builder.Property(u => u.EncryptedAESKey);

            builder.Property(u => u.Salt);

            builder.HasIndex(u => u.Username).IsUnique();
        }
    }
}
