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
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;

using sd = System.Drawing;

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

        public async Task<Response<IList<Vault>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var data = await GetByCondition(vault => vault.UserId == userId && !vault.Deleted).ToListAsync(cancellationToken);

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

        public async Task<Response<Vault>> GetByIdAsync(Guid vaultId, CancellationToken cancellationToken)
        {
            var data = await GetByCondition(vault => vault.Id == vaultId && !vault.Deleted).FirstOrDefaultAsync(cancellationToken);

            if (data != null)
            {
                return new Response<Vault>(
                    data: data,
                    succeeded: true,
                    message: "The vaults have been successfully retrieved!",
                    statusCode: (int)HttpStatusCode.OK
                );
            }

            return new Response<Vault>(
                data: null,
                succeeded: false,
                message: "No vaults could be found for the specified Id.",
                statusCode: (int)HttpStatusCode.NotFound
            );
        }

        public async Task<Response<Vault>> UpdateAsync(Guid vaultId, UpdateVaultDto vaultDto, CancellationToken cancellationToken)
        {
            var existingVault = await GetByCondition(vault => vault.Id == vaultId && !vault.Deleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingVault == null)
            {
                return new Response<Vault>(
                    data: null,
                    succeeded: false,
                    message: "No vaults could be found for the specified Id.",
                    statusCode: (int)HttpStatusCode.NotFound
                );
            }

            if (!string.Equals(existingVault.Url, vaultDto.Url, StringComparison.OrdinalIgnoreCase))
            {
                var faviconData = await FetchFaviconAsync(vaultDto.Url, cancellationToken);
                if (faviconData != null)
                {
                    existingVault.FavIcon = faviconData;
                }
                else
                {
                    existingVault.FavIcon = null;
                }
            }

            existingVault.Title = vaultDto.Title;
            existingVault.Username = vaultDto.Username;

            if (!string.IsNullOrEmpty(vaultDto.EncryptedPassword))
            {
                existingVault.EncryptedPassword = vaultDto.EncryptedPassword;
                if(existingVault.PasswordHistory == null)
                {
                    existingVault.PasswordHistory = 2;
                }
                else
                {
                    existingVault.PasswordHistory++;
                }
            }

            existingVault.Url = vaultDto.Url;
            existingVault.ChangedOn = DateTime.UtcNow;

            try
            {
                var updatedVault = await UpdateAsync(existingVault, cancellationToken);

                if (updatedVault != null)
                {
                    return new Response<Vault>(
                        data: updatedVault,
                        succeeded: true,
                        message: "The vault has been successfully updated!",
                        statusCode: (int)HttpStatusCode.OK
                    );
                }

                return new Response<Vault>(
                    data: null,
                    succeeded: false,
                    message: "Failed to update the vault.",
                    statusCode: (int)HttpStatusCode.InternalServerError
                );
            }
            catch (Exception ex)
            {
                return new Response<Vault>(
                    data: null,
                    succeeded: false,
                    message: "An error occurred while updating the vault.",
                    statusCode: (int)HttpStatusCode.InternalServerError
                );
            }
        }

        private async Task<byte[]?> FetchFaviconAsync(string? url, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var baseUri))
            {
                throw new UriFormatException("The provided URL is not a valid absolute URI.");
            }

            var faviconUrl = new Uri(baseUri, "/favicon.ico").ToString();

            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(faviconUrl, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var faviconBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
                    return ResizeImage(faviconBytes, 20, 20);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }


        private byte[]? ResizeImage(byte[] imageBytes, int width, int height)
        {
            try
            {
                using var ms = new MemoryStream(imageBytes);
                using var originalImage = sd.Image.FromStream(ms);
                var resizedImage = new Bitmap(width, height);

                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.CompositingQuality = sd.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = sd.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = sd.Drawing2D.SmoothingMode.HighQuality;

                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                using var outputStream = new MemoryStream();
                resizedImage.Save(outputStream, sd.Imaging.ImageFormat.Png);
                return outputStream.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
