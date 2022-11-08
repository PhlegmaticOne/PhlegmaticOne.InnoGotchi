namespace PhlegmaticOne.InnoGotchi.Web.Services.Storage;

public static class LocalStorageExtensions
{
    private const string JwtTokenKey = "JwtToken";
    private const string ServerAddressKey = "ServerAddress";
    

    public static void SetJwtToken(this ILocalStorageService localStorageService, string jwtToken) => 
        localStorageService.SetValue(JwtTokenKey, jwtToken);

    public static string? GetJwtToken(this ILocalStorageService localStorageService) => 
        localStorageService.GetValue<string>(JwtTokenKey);

    public static void SetServerAddress(this ILocalStorageService localStorageService, string serverAddress) => 
        localStorageService.SetValue(ServerAddressKey, new Uri(serverAddress));

    public static Uri? GetServerAddress(this ILocalStorageService localStorageService) => 
        localStorageService.GetValue<Uri>(ServerAddressKey);
}