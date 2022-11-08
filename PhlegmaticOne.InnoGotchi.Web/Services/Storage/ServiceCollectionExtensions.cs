﻿namespace PhlegmaticOne.InnoGotchi.Web.Services.Storage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalStorage(this IServiceCollection serviceCollection) =>
        serviceCollection.AddSingleton<ILocalStorageService, InMemoryLocalStorageService>();

    public static IServiceCollection AddLocalStorage(this IServiceCollection serviceCollection,
        Action<ILocalStorageService> startConfigurationAction) =>
        serviceCollection.AddSingleton<ILocalStorageService>(x =>
        {
            var storage = new InMemoryLocalStorageService();
            startConfigurationAction(storage);
            return storage;
        });
}