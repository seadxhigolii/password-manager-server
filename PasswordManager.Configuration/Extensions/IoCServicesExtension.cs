using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PasswordManager.Services.Interfaces;
using PasswordManager.Services.Interfaces.Decryption;
using PasswordManager.Services.Interfaces.Encryption;
using PasswordManager.Services.Interfaces.Shared;
using PasswordManager.Services.Services;
using PasswordManager.Services.Services.Decryption;
using PasswordManager.Services.Services.Encryption;
using PasswordManager.Services.Services.Shared;

namespace PasswordManager.Configuration.Extensions
{
    public static class IoCServicesExtension
    {
        public static void RegisterIoCServices(this IServiceCollection services)
        {
            #region Shared

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();

            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
            services.AddSingleton<IWebHelper, WebHelper>();

            //services.AddScoped<ILogService, LogService>();
            //services.AddScoped<IHttpClientService, HttpClientService>();
            //services.AddScoped<IEmailSenderService, EmailSenderService>();

            #endregion Shared


            #region Api

            services.AddScoped<IAuthService, AuthService>();

            #endregion Api



            #region Encryption

            services.AddScoped<IEncryptionService, EncryptionService>();

            #endregion Encryption  



            #region Decryption

            services.AddScoped<IDecryptionService, DecryptionService>();

            #endregion Decryption
        }
    }
}
