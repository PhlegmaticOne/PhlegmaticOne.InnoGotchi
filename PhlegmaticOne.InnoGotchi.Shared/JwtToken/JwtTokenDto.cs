namespace PhlegmaticOne.InnoGotchi.Shared.JwtToken;

public class JwtTokenDto
{
    public string? Token { get; init; }

    public JwtTokenDto(string token)
    {
        Token = token;
    }
}