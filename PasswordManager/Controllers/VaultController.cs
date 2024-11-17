using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Controllers.Shared;
using PasswordManager.Core.Domain;
using PasswordManager.Core.Dto.Requests.VaultDtos;
using PasswordManager.Core.Shared;
using PasswordManager.Services.Interfaces;

namespace PasswordManager.Api.Controllers
{
    public class VaultController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IVaultService _vaultService;

        public VaultController(IConfiguration configuration, IAuthService authService, IVaultService vaultService)
        {
            _configuration = configuration;
            _authService = authService;
            _vaultService = vaultService;
        }

        [HttpPost("Create")]
        public async Task<Response<bool>> Create([FromBody] CreateVaultDto createVaultDto, CancellationToken cancellationToken)
        {
            var result = await _vaultService.CreateAsync(createVaultDto, cancellationToken);
            return result;
        }

        [HttpGet("GetAllByUserId/{userId}")]
        public async Task<Response<IList<Vault>>> GetAllByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _vaultService.GetByUserIdAsync(userId, cancellationToken);
            return result;
        }

        [HttpGet("GetById/{vaultId}")]
        public async Task<Response<Vault>> GetById(Guid vaultId, CancellationToken cancellationToken)
        {
            var result = await _vaultService.GetByIdAsync(vaultId, cancellationToken);
            return result;
        }

        [HttpPut("GetById/{vaultId}")]
        public async Task<Response<bool>> Update(Guid vaultId, [FromBody] UpdateVaultDto vault, CancellationToken cancellationToken)
        {
            var result = await _vaultService.UpdateAsync(vaultId, vault, cancellationToken);
            return result;
        }
    }
}
