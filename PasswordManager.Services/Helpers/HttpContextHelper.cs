using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace PasswordManager.Services.Helpers;

public class HttpContextHelper
{
    #region Properties

    private static IHttpContextAccessor _httpContextAccessor;

    #endregion Properties

    #region Methods

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public static HttpContext CurrentContext => _httpContextAccessor?.HttpContext;

    public static IConfiguration CurrentConfiguration => CurrentContext.RequestServices.GetService<IConfiguration>();


    public static string? GetUserId()
    {
        return CurrentContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    #endregion Methods
}
