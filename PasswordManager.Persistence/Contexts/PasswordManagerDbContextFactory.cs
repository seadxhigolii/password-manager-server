using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PasswordManager.Persistence.Contexts
{
    public class PasswordManagerDbContextFactory : IDesignTimeDbContextFactory<PasswordManagerDbContext>
    {
        public PasswordManagerDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PasswordManagerDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PasswordManagerDatabase"));

            return new PasswordManagerDbContext(optionsBuilder.Options);
        }
    }
}
