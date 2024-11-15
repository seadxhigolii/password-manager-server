using PasswordManager.Core.Domain;
using PasswordManager.Core.Dto.Requests;
using PasswordManager.Core.Shared;
using PasswordManager.Services.Interfaces.Shared;

namespace PasswordManager.Services.Interfaces
{
    public interface IVaultService : IGenericService<Vault, Guid>
    {
        Task<Response<bool>> CreateAsync(CreateVaultDto vault, CancellationToken cancellationToken);
    }
}
