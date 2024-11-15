using PasswordManager.Core.Domain;
using PasswordManager.Core.Dto.Shared;
using PasswordManager.Core.Enum;

namespace PasswordManager.Core.Dto.Requests
{
    public class CreateVaultDto : BaseDto<CreateVaultDto, Vault>
    {
        public Guid UserId { get; set; }
        public VaultItemTypeEnum ItemType { get; set; }
        public string Title { get; set; }
        public string EncryptedPassword { get; set; }        
        public string Username { get; set; } 

    }
}
