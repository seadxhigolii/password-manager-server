using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Controllers.Shared;
using PasswordManager.Core.Dto;
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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            return Ok(new { Message = "User registered successfully!" });
        }

        [HttpPost("Login")]
        public async Task<Response<string>> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            if (loginDto.Username != "test" || loginDto.Password != "password")
            {
                return new Response<string>
                {
                    StatusCode = 500
                };
            }

            var result = await _authService.GenerateJwtToken(loginDto.Username, cancellationToken);

            return result;
        }      
    }
}
