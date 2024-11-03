using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Core.Configuration
{
    public static class ModelBuilderConfiguration
    {
        public static void ApplyEntityTypes(this ModelBuilder builder, Assembly assembly, Type configInterface)
        {
            var typesToRegister = assembly.GetTypes().Where(t => t.GetInterfaces()
                .Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == configInterface))
                .Where(x => !x.IsAbstract).ToList();

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                builder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}
