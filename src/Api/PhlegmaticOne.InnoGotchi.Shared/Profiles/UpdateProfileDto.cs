namespace PhlegmaticOne.InnoGotchi.Shared.Profiles;

public class UpdateProfileDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
}