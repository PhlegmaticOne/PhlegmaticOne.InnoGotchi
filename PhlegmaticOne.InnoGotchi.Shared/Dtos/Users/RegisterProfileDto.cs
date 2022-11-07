namespace PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

public class RegisterProfileDto : UserDtoBase
{
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    //public byte[] AvatarData { get; set; }
}