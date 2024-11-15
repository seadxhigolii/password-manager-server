using PasswordManager.Core.Dto.Requests;
using PasswordManager.Core.Dto.Responses;
using PasswordManager.Core.Shared;

namespace PasswordManager.Services.Interfaces
{
    public interface IAuthService
    {
        //Task<Response<AuthDto>> Login(LoginDto userLoginDto, CancellationToken cancellationToken);
        Task<Response<UserRegisteredDto>> Register(RegisterDto model, CancellationToken cancellationToken);
        Task<Response<UserLoggedInDto>> Login(LoginDto model, CancellationToken cancellationToken);

        //Task<Response<string>> GenerateJwtToken(string username, CancellationToken cancellationToken);
        //Task<Response<string>> DecodeToken(string token, CancellationToken cancellationToken);
        //Task<Response<bool>> VerifyTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}
