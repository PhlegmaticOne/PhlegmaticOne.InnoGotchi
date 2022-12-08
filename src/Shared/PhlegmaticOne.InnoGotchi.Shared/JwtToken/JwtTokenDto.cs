namespace PhlegmaticOne.InnoGotchi.Shared.JwtToken;

public class JwtTokenDto
{
    public JwtTokenDto(string token)
    {
        Token = token;
    }

    public string? Token { get; init; }
}