namespace PhlegmaticOne.InnoGotchi.Web.Services.Storage;

public static class LocalStorageExtensions
{
    private const string JwtTokenKey = "JwtToken";
    private const string ServerAddressKey = "ServerAddress";
    private const string AnonymousEndpointsKey = "AnonymousEndpoints";
    private const string IsAuthenticationRequiredKey = "IsAuthenticationRequired";
    private const string LoginUrlKey = "LoginUrl";
    
    public static void SetJwtToken(this ILocalStorageService localStorageService, string jwtToken) => 
        localStorageService.SetValue(JwtTokenKey, jwtToken);

    public static string? GetJwtToken(this ILocalStorageService localStorageService) => 
        localStorageService.GetValue<string>(JwtTokenKey);

    public static void SetServerAddress(this ILocalStorageService localStorageService, string serverAddress) => 
        localStorageService.SetValue(ServerAddressKey, new Uri(serverAddress));

    public static Uri? GetServerAddress(this ILocalStorageService localStorageService) => 
        localStorageService.GetValue<Uri>(ServerAddressKey);

    public static void SetAnonymousEndpoints(this ILocalStorageService localStorageService, params string[] anonymousEndpoints) =>
        localStorageService.SetValue(AnonymousEndpointsKey, anonymousEndpoints.ToArray());

    public static string[]? GetAnonymousEndpoints(this ILocalStorageService localStorageService) =>
        localStorageService.GetValue<string[]>(AnonymousEndpointsKey);

    public static void SetIsAuthenticationRequired(this ILocalStorageService localStorageService, bool isAuthenticationRequired) =>
        localStorageService.SetValue(IsAuthenticationRequiredKey, isAuthenticationRequired);

    public static bool? GetIsAuthenticationRequired(this ILocalStorageService localStorageService) =>
        localStorageService.GetValue<bool>(IsAuthenticationRequiredKey);

    public static void SetLoginUrl(this ILocalStorageService localStorageService, string loginUrl) =>
        localStorageService.SetValue(LoginUrlKey, loginUrl);

    public static string? GetLoginUrl(this ILocalStorageService localStorageService) =>
        localStorageService.GetValue<string>(LoginUrlKey);
}