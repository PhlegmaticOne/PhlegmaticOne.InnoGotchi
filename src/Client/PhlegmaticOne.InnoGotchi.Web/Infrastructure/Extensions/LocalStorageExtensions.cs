using PhlegmaticOne.LocalStorage;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;

public static class LocalStorageExtensions
{
    private const string JwtTokenKey = "JwtToken";
    private const string LoginPathKey = "LoginPath";

    public static void SetJwtToken(this ILocalStorageService localStorageService, string jwtToken) => 
        localStorageService.SetValue(JwtTokenKey, jwtToken);

    public static string? GetJwtToken(this ILocalStorageService localStorageService) =>
        localStorageService.GetValue<string>(JwtTokenKey);

    public static void SetLoginPath(this ILocalStorageService localStorageService, string loginPath) => 
        localStorageService.SetValue(LoginPathKey, loginPath);

    public static string? GetLoginPath(this ILocalStorageService localStorageService) => 
        localStorageService.GetValue<string>(LoginPathKey);
}