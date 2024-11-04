using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Core.Dto
{
    public class AuthDto
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool HasAcceptedDisclaimer { get; set; }
        public string UserLangauge { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string UserGuid { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public bool InActiveDirectory { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
