using PhlegmaticOne.InnoGotchi.Shared.JwtToken;

namespace PhlegmaticOne.InnoGotchi.Shared.Profiles;

public class AuthorizedProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public JwtTokenDto JwtToken { get; set; } = null!;
}