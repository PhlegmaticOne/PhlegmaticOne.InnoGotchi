namespace PhlegmaticOne.InnoGotchi.Shared.Dtos;

public class JwtTokenDto
{
    public string? Token { get; init; }

    public JwtTokenDto(string token)
    {
        Token = token;
    }
}