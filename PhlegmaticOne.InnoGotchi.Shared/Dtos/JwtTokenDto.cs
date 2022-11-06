namespace PhlegmaticOne.InnoGotchi.Shared.Dtos;

public class JwtTokenDto
{
    public string? Token { get; init; }
    public bool IsAuthenticated { get; init; }

    public static JwtTokenDto FromToken(string token) => new()
    {
        Token = token,
        IsAuthenticated = true
    };

    public static JwtTokenDto Failed => new()
    {
        IsAuthenticated = false
    };
}