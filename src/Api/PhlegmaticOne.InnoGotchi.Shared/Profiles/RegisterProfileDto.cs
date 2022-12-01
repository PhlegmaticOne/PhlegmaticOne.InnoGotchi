namespace PhlegmaticOne.InnoGotchi.Shared.Profiles;

public class RegisterProfileDto : IdentityDtoBase
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
}