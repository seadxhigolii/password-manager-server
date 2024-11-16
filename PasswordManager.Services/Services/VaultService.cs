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

            var faviconData = await FetchFaviconAsync(entity.Url, cancellationToken);

            if (faviconData != null)
            {
                vault.FavIcon = faviconData;
            }

            var data = await CreateAsync(vault, cancellationToken);

            if (data != null)
            {
                return new Response<bool>(
                    data: true,
                    succeeded: true,
                    message: "The vault has been successfully added!",
                    statusCode: (int)HttpStatusCode.OK
                );
            }

            return new Response<bool>(
                data: false,
                succeeded: false,
                message: "Failed to add the vault.",
                statusCode: (int)HttpStatusCode.InternalServerError
            );
            
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

        private async Task<byte[]?> FetchFaviconAsync(string? url, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            try
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "http://" + url;
                }

                if (!Uri.TryCreate(url, UriKind.Absolute, out var baseUri))
                {
                    throw new UriFormatException("The provided URL is not a valid absolute URI.");
                }

                var faviconUrl = new Uri(baseUri, "/favicon.ico").ToString();

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(faviconUrl, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching favicon: {ex.Message}");
            }

            return null;
        }



    }
}
