using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Core.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Persistence.Configuration.Shared
{
    public abstract class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<TId>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedOn)
                   .IsRequired();

            builder.Property(e => e.CreatedBy)
                   .HasMaxLength(100);

            builder.Property(e => e.ChangedOn)
                   .IsRequired(false);

            builder.Property(e => e.ChangedBy)
                   .HasMaxLength(100);
        }
    }
}
