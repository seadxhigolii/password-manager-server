using PasswordManager.Core.Dto;
using PasswordManager.Core.Shared;

namespace PasswordManager.Services.Interfaces
{
    public interface IAuthService
    {
        //Task<Response<AuthDto>> Login(LoginDto userLoginDto, CancellationToken cancellationToken);
        //Task<bool> Register(LoginDto model);
        Task<Response<string>> GenerateJwtToken(string username, CancellationToken cancellationToken);
        //Task<Response<string>> DecodeToken(string token, CancellationToken cancellationToken);
        //Task<Response<bool>> VerifyTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}
