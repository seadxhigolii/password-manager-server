using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Controllers.Shared;
using PasswordManager.Core.Dto.Requests;
using PasswordManager.Core.Dto.Responses;
using PasswordManager.Core.Shared;
using PasswordManager.Services.Interfaces;

namespace PasswordManager.Api.Controllers
{
    public class AuthController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<Response<UserRegisteredDto>> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var result = await _authService.Register(registerDto, cancellationToken);
            return result;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<Response<UserLoggedInDto>> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            var result = await _authService.Login(loginDto, cancellationToken);

            return result;
        }      
    }
}
