namespace PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

public class IdentityUserDto
{
    public IdentityUserDto(string email)
    {
        Email = email;
    }
    public string Email { get; init; }
}