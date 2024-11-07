using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Core.Domain;
using PasswordManager.Persistence.Configuration.Shared;

namespace PasswordManager.Persistence.Configuration
{
    public class CustomFieldConfiguration : BaseEntityConfiguration<CustomField, Guid>
    {
        public override void Configure(EntityTypeBuilder<CustomField> builder)
        {
            base.Configure(builder);

            builder.Property(cf => cf.FieldName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(cf => cf.EncryptedFieldValue)
                   .IsRequired();

            builder.HasOne(cf => cf.Vault)
                   .WithMany(v => v.CustomFields)
                   .HasForeignKey(cf => cf.VaultId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cf => cf.VaultId);
        }
    }
}
