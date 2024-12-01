using PasswordManager.Core.Dto.Shared;
using PasswordManager.Core.Enum;


namespace PasswordManager.Core.Dto.Requests.VaultDtos
{
    public class CreateVaultDto : BaseDto<CreateVaultDto, Domain.Vault>
    {
        public Guid UserId { get; set; }
        public VaultItemTypeEnum ItemType { get; set; }
        public string Title { get; set; }
        public string? EncryptedPassword { get; set; }
        public string? Username { get; set; }
        public string? EncryptedNotes { get; set; }
        public string? Url { get; set; }

    }
}
