namespace PhlegmaticOne.InnoGotchi.Web.Services.Storage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalStorage(this IServiceCollection serviceCollection) =>
        serviceCollection.AddSingleton<ILocalStorageService, InMemoryLocalStorageService>();

    public static IServiceCollection AddLocalStorage(this IServiceCollection serviceCollection, string serverAddress) =>
        serviceCollection.AddSingleton<ILocalStorageService>(x =>
        {
            var storage = new InMemoryLocalStorageService();
            storage.SetServerAddress(serverAddress);
            return storage;
        });
}