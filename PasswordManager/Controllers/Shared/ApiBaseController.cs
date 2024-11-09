using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Persistence.Contexts;

namespace PasswordManager.Api.Controllers.Shared
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiBaseController : ControllerBase
    {
        private readonly PasswordManagerDbContext _context;
        public ApiBaseController()
        {
            
        }
    }
}
