using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Domain;
using PasswordManager.Core.Dto.Requests.VaultDtos;
using PasswordManager.Core.Shared;
using PasswordManager.Persistence.Contexts;
using PasswordManager.Services.Interfaces;
using PasswordManager.Services.Interfaces.Decryption;
using PasswordManager.Services.Interfaces.Encryption;
using PasswordManager.Services.Services.Shared;
using System.Net;

namespace PasswordManager.Services.Services
{
    public class VaultService : GenericService<Vault, Guid>, IVaultService
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordManagerDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        private readonly IDecryptionService _decryptionService;

        public VaultService(IConfiguration configuration,
            PasswordManagerDbContext dbContext,
            IEncryptionService encryptionService,
            IDecryptionService decryptionService) : base(dbContext, configuration)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _decryptionService = decryptionService;
            _configuration = configuration;
        }

        public async Task<Response<bool>> CreateAsync(CreateVaultDto entity, CancellationToken cancellationToken)
        {
            var vault = entity.ToEntity();
            var data = await CreateAsync(
                                          entity: vault,
                                          cancellationToken: cancellationToken
                                        );
            if (data != null)
                return new Response<bool>(data: true, succeeded: true, message: "The vault has been successfully added!", statusCode: (int)HttpStatusCode.OK);

            return new Response<bool>(data: false, succeeded: false, message: "", statusCode: (int)HttpStatusCode.InternalServerError);
        }
        public async Task<Response<IList<Vault>>> GetByUserId(GetVaultsByUserId entity, CancellationToken cancellationToken)
        {
            var data = await GetByCondition(vault => vault.UserId == entity.UserId).ToListAsync(cancellationToken);

            if (data.Any())
            {
                return new Response<IList<Vault>>(
                    data: data,
                    succeeded: true,
                    message: "The vaults have been successfully retrieved!",
                    statusCode: (int)HttpStatusCode.OK
                );
            }

            return new Response<IList<Vault>>(
                data: null,
                succeeded: false,
                message: "No vaults could be found for the specified user.",
                statusCode: (int)HttpStatusCode.NotFound
            );
        }

    }
}
