namespace PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

public class ProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] AvatarData { get; set; } = null!;
    public JwtTokenDto JwtToken { get; set; } = null!;
}