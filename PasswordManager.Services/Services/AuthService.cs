using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Core.Shared;
using PasswordManager.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PasswordManager.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Response<string>> GenerateJwtToken(string username, CancellationToken cancellationToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            var response = new Response<string>
            {
                Data = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return response;
        }
    }
}
