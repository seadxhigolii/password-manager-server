using Microsoft.Net.Http.Headers;
using PasswordManager.Services.Helpers;
using PasswordManager.Services.Interfaces;
using System.Net;

namespace PasswordManager.Services.Services
{
    public class WebHelper : IWebHelper
    {
        public WebHelper()
        {
        }

        public bool IsRequestAvailable()
        {
            if (HttpContextHelper.CurrentContext == null)
                return false;

            try
            {
                if (HttpContextHelper.CurrentContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            var result = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(result) && HttpContextHelper.CurrentContext.Connection.RemoteIpAddress != null)
                    result = HttpContextHelper.CurrentContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }

            if (result != null && result.Equals(IPAddress.IPv6Loopback.ToString(), StringComparison.InvariantCultureIgnoreCase))
                result = IPAddress.Loopback.ToString();

            if (IPAddress.TryParse(result ?? string.Empty, out var ip))
                result = ip.ToString();
            else if (!string.IsNullOrEmpty(result))
                result = result.Split(':').FirstOrDefault();

            return result;
        }

        public string GetThisPageUrl()
        {
            var pageUrl = (HttpContextHelper.CurrentContext is not null) ? $"{HttpContextHelper.CurrentContext.Request.Host}{HttpContextHelper.CurrentContext.Request.Path}{HttpContextHelper.CurrentContext.Request.QueryString}" : "";
            return pageUrl;
        }

        public string GetUrlReferrer()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            return HttpContextHelper.CurrentContext.Request.Headers[HeaderNames.Referer];
        }
    }
}
