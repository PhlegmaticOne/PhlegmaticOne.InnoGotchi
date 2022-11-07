namespace PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

public class ProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Email { get; set; }
    public byte[] AvatarData { get; set; }
    public JwtTokenDto JwtToken { get; set; }
}