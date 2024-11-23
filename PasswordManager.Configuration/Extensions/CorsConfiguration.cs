using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace PasswordManager.Configuration.Extensions
{
    public static class CorsConfiguration
    {
        public static void AddCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }
    }
}
