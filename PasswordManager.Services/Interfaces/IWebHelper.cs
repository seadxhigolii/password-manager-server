namespace PasswordManager.Services.Interfaces
{
    public interface IWebHelper
    {
        bool IsRequestAvailable();

        string GetCurrentIpAddress();

        string GetThisPageUrl();

        string GetUrlReferrer();
    }
}
