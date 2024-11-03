using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordManager.Core.Domain;
using PasswordManager.Persistence.Configuration.Shared;

namespace PasswordManager.Persistence.Configuration
{
    public class LogConfiguration : BaseEntityConfiguration<Log, Guid>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.Property(e => e.Level)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Message)
                   .IsRequired();

            builder.Property(e => e.Properties)
                   .HasConversion(
                       v => v == null ? "{}" : v,
                       v => v
                   );

            builder.Property(e => e.LogEvent)
                   .HasConversion(
                       v => v == null ? "{}" : v,
                       v => v
                   );
        }
    }
}
