using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Core.Domain;
using PasswordManager.Core.Dto.Requests;
using PasswordManager.Core.Dto.Responses;
using PasswordManager.Core.Shared;
using PasswordManager.Persistence.Contexts;
using PasswordManager.Services.Helpers;
using PasswordManager.Services.Interfaces;
using PasswordManager.Services.Interfaces.Decryption;
using PasswordManager.Services.Interfaces.Encryption;
using PasswordManager.Services.Services.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services.Services
{
    public class AuthService : GenericService<User, Guid>, IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordManagerDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        private readonly IDecryptionService _decryptionService;

        public AuthService(IConfiguration configuration, 
            PasswordManagerDbContext dbContext,
            IEncryptionService encryptionService,
            IDecryptionService decryptionService) : base(dbContext, configuration)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _decryptionService = decryptionService; 
            _configuration = configuration;
        }

        public async Task<Response<UserRegisteredDto>> Register(RegisterDto model, CancellationToken cancellationToken)
        {
            using (var rsa = RSA.Create(4096))
            {
                var publicKey = rsa.ExportRSAPublicKey();
                var privateKey = rsa.ExportRSAPrivateKey();

                var salt = Generator.GenerateSalt();
                var hashedMasterPassword = _encryptionService.HashPassword(model.MasterPassword, salt);

                var derivedKey = _decryptionService.DeriveKeyFromPassword(model.MasterPassword, salt);
                var aesKey = Generator.GenerateAESKey();
                var encryptedPrivateKey = _encryptionService.EncryptWithAES(privateKey, aesKey);
                var encryptedAESKey = _encryptionService.EncryptWithAES(aesKey, derivedKey);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = model.Username,
                    PublicKey = Convert.ToBase64String(publicKey),
                    EncryptedPrivateKey = Convert.ToBase64String(encryptedPrivateKey),
                    EncryptedAESKey = Convert.ToBase64String(encryptedAESKey),
                    Salt = Convert.ToBase64String(salt),
                    MasterPassword = Convert.ToBase64String(hashedMasterPassword)
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var result = new UserRegisteredDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    PublicKey = user.PublicKey,
                    EncryptedPrivateKey = user.EncryptedPrivateKey,
                    EncryptedAESKey = user.EncryptedAESKey,
                    Salt = user.Salt
                };

                var response = new Response<UserRegisteredDto>
                {
                    Data = result
                };

                return response;
            }
        }

        public async Task<Response<UserLoggedInDto>> Login(LoginDto model, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
            {
                return new Response<UserLoggedInDto>
                {
                    Message = "Invalid username or password."
                };
            }

            var salt = Convert.FromBase64String(user.Salt);
            var hashedInputPassword = _encryptionService.HashPassword(model.Password, salt);

            if (!hashedInputPassword.SequenceEqual(Convert.FromBase64String(user.MasterPassword)))
            {
                return new Response<UserLoggedInDto>
                {
                    Message = "Invalid username or password."
                };
            }

            var derivedKey = _decryptionService.DeriveKeyFromPassword(model.Password, salt);

            var encryptedAESKey = Convert.FromBase64String(user.EncryptedAESKey);
            var aesKey = _decryptionService.DecryptWithAES(encryptedAESKey, derivedKey);
            var encryptedPrivateKey = Convert.FromBase64String(user.EncryptedPrivateKey);
            var privateKey = _decryptionService.DecryptWithAES(encryptedPrivateKey, aesKey);

            var authToken = GenerateJwtToken(user.Username, CancellationToken.None);

            var result = new UserLoggedInDto
            {
                UserId = user.Id,
                AuthToken = authToken,
                PrivateKey = Convert.ToBase64String(privateKey),
                Username = user.Username,
                PublicKey = user.PublicKey
            };

            return new Response<UserLoggedInDto>
            {
                Data = result
            };
        }

        public string GenerateJwtToken(string username, CancellationToken cancellationToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            if (jwtSettings == null)
            {
                throw new ArgumentNullException(nameof(jwtSettings), "JwtSettings configuration section is missing.");
            }

            string secretKey = jwtSettings["SecretKey"];
            string issuer = jwtSettings["Issuer"];
            string audience = jwtSettings["Audience"];
            string expiresInMinutes = jwtSettings["ExpiresInMinutes"];

            // Ensure none of the required settings are null
            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) ||
                string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(expiresInMinutes))
            {
                throw new ArgumentNullException("One or more JwtSettings values are missing or null.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(expiresInMinutes)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
