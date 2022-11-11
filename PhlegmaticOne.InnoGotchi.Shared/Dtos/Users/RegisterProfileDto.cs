namespace PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

public class RegisterProfileDto : UserDtoBase
{
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
}