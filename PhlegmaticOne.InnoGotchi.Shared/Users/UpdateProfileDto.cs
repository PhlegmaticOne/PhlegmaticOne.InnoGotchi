namespace PhlegmaticOne.InnoGotchi.Shared.Users;

public class UpdateProfileDto
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
}