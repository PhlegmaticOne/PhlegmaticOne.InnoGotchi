using PhlegmaticOne.InnoGotchi.Shared.Profiles.Base;

namespace PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;

public class RegisterProfileDto : IdentityDtoBase
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
}