namespace PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

public class RegisterProfileDto : UserDtoBase
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public byte[] AvatarData { get; set; }
}