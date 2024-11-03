using Microsoft.EntityFrameworkCore;
using PasswordManager.Core.Domain;
using PasswordManager.Core.Configuration;

namespace PasswordManager.Persistence.Contexts
{
    public class PasswordManagerDbContext : DbContext
    {
        public PasswordManagerDbContext(DbContextOptions<PasswordManagerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyEntityTypes(typeof(PasswordManagerDbContext).Assembly, typeof(IEntityTypeConfiguration<>));
        }

        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordEntry> PasswordEntries { get; set; }
        public DbSet<Log> Logs { get; set; }

        #endregion DbSet
    }
}
