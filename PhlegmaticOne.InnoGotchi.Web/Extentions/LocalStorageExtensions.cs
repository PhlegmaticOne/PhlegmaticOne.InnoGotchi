using PhlegmaticOne.LocalStorage.Base;

namespace PhlegmaticOne.InnoGotchi.Web.Extentions;

public static class LocalStorageExtensions
{
    private const string JwtTokenKey = "JwtToken";
    private const string ServerAddressKey = "ServerAddress";
    private const string LoginPathKey = "LoginPath";
    private const string ErrorPathKey = "ErrorPath";
    public static void SetJwtToken(this ILocalStorageService localStorageService, string jwtToken) =>
        localStorageService.SetValue(JwtTokenKey, jwtToken);

    public static string? GetJwtToken(this ILocalStorageService localStorageService) =>
        localStorageService.GetValue<string>(JwtTokenKey);

    public static void SetServerAddress(this ILocalStorageService localStorageService, string serverAddress) =>
        localStorageService.SetValue(ServerAddressKey, new Uri(serverAddress));

    public static Uri? GetServerAddress(this ILocalStorageService localStorageService) =>
        localStorageService.GetValue<Uri>(ServerAddressKey);

    public static void SetLoginPath(this ILocalStorageService localStorageService, string loginPath) =>
        localStorageService.SetValue(LoginPathKey, loginPath);

    public static string? GetLoginPath(this ILocalStorageService localStorageService) =>
        localStorageService.GetValue<string>(LoginPathKey);

    public static void SetErrorPath(this ILocalStorageService localStorageService, string errorPath) =>
        localStorageService.SetValue(ErrorPathKey, errorPath);

    public static string? GetErrorPath(this ILocalStorageService localStorageService) =>
        localStorageService.GetValue<string>(ErrorPathKey);
}