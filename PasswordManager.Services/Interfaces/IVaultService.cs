using PasswordManager.Core.Domain;
using PasswordManager.Core.Dto.Requests.VaultDtos;
using PasswordManager.Core.Shared;
using PasswordManager.Services.Interfaces.Shared;

namespace PasswordManager.Services.Interfaces
{
    public interface IVaultService : IGenericService<Vault, Guid>
    {
        Task<Response<bool>> CreateAsync(CreateVaultDto vault, CancellationToken cancellationToken);
        Task<Response<IList<Vault>>> GetByUserId(GetVaultsByUserId entity, CancellationToken cancellationToken);
        Task<Response<Vault>> GetById(GetVaultById entity, CancellationToken cancellationToken);
    }
}
