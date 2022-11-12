namespace PhlegmaticOne.InnoGotchi.Shared.Users;

public class UpdateProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
}