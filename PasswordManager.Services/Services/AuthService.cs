using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Domain;
using PasswordManager.Core.Dto;
using PasswordManager.Core.Shared;
using PasswordManager.Persistence.Contexts;
using PasswordManager.Services.Helpers;
using PasswordManager.Services.Interfaces;
using PasswordManager.Services.Interfaces.Decryption;
using PasswordManager.Services.Interfaces.Encryption;
using PasswordManager.Services.Services.Shared;
using System.Security.Cryptography;

namespace PasswordManager.Services.Services
{
    public class AuthService : GenericService<User, Guid>, IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordManagerDbContext _dbContext;
        private readonly IEncryptionService _encrpytionService;
        private readonly IDecryptionService _decryptionService;

        public AuthService(IConfiguration configuration, 
            PasswordManagerDbContext dbContext,
            IEncryptionService encryptionService,
            IDecryptionService decryptionService) : base(dbContext, configuration)
        {
            _dbContext = dbContext;
            _encrpytionService = encryptionService;
            _decryptionService = decryptionService;
        }

        public async Task<bool> Register(RegisterDto model)
        {
            using (var rsa = RSA.Create(4096)) // 4096-bit RSA for maximum security
            {
                var publicKey = rsa.ExportRSAPublicKey();
                var privateKey = rsa.ExportRSAPrivateKey();

                var salt = Generator.GenerateSalt();

                var derivedKey = _decryptionService.DeriveKeyFromPassword(model.MasterPassword, salt);

                var aesKey = Generator.GenerateAESKey();
                var encryptedPrivateKey = _encrpytionService.EncryptWithAES(privateKey, aesKey);

                var encryptedAESKey = _encrpytionService.EncryptWithAES(aesKey, derivedKey);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = model.Username,
                    PublicKey = Convert.ToBase64String(publicKey),
                    EncryptedPrivateKey = Convert.ToBase64String(encryptedPrivateKey),
                    EncryptedAESKey = Convert.ToBase64String(encryptedAESKey),
                    Salt = Convert.ToBase64String(salt)
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return true;
            }
        }

        public async Task<Response<string>> GenerateJwtToken(string username, CancellationToken cancellationToken)
        {
            var token = Generator.GenerateJwtToken(_configuration,username, cancellationToken);

            return new Response<string>
            {
                Data = token
            };
        }
    }
}
