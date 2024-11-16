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

        public VaultController(IAuthService authService)
        {
            _authService = authService;
        }

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

        [HttpPost("GetByUserId")]
        public async Task<Response<IList<Vault>>> GetByUserId([FromBody] GetVaultsByUserId entity, CancellationToken cancellationToken)
        {
            var result = await _vaultService.GetByUserId(entity, cancellationToken);
            return result;
        }
    }
}
