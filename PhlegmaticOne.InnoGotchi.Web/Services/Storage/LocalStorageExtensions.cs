namespace PhlegmaticOne.InnoGotchi.Web.Services.Storage;

public static class LocalStorageExtensions
{
    private const string JwtTokenKey = "JwtToken";
    public static IServiceCollection AddLocalStorage(this IServiceCollection serviceCollection) => 
        serviceCollection.AddSingleton<ILocalStorageService, InMemoryLocalStorageService>();

    public static void SetJwtToken(this ILocalStorageService localStorageService, string jwtToken)
    {
        localStorageService.SetValue(JwtTokenKey, jwtToken);
    }

    public static string GetJwtToken(this ILocalStorageService localStorageService)
    {
        return localStorageService.GetValue<string>(JwtTokenKey);
    }
}