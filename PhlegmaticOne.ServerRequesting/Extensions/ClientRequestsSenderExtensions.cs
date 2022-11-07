﻿using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.ServerRequesting.Builders;
using PhlegmaticOne.ServerRequesting.Implementation;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.ServerRequesting.Extensions;

public static class ClientRequestsSenderExtensions
{
    private const string HttpClientName = "Server";
    public static IServiceCollection AddClientRequestsService(this IServiceCollection serviceCollection,
        string serverAddress, Action<ClientRequestsBuilder> builderAction)
    {
        var clientRequestsBuilder = new ClientRequestsBuilder();
        builderAction(clientRequestsBuilder);

        ConfigureHttpClient(serviceCollection, serverAddress);

        AddClientRequestsService(serviceCollection, clientRequestsBuilder);

        return serviceCollection;
    }

    private static void ConfigureHttpClient(IServiceCollection serviceCollection, string serverAddress)
    {
        serviceCollection.AddHttpClient(HttpClientName, httpClient =>
        {
            httpClient.BaseAddress = new Uri(serverAddress);
        });
    }

    private static void AddClientRequestsService(IServiceCollection serviceCollection, ClientRequestsBuilder clientRequestsBuilder)
    {
        var requestUrls = clientRequestsBuilder.Build();
        serviceCollection.AddScoped<IClientRequestsService>(x =>
        {
            var httpClientFactory = x.GetRequiredService<IHttpClientFactory>();
            return new ClientRequestsService(httpClientFactory, requestUrls, HttpClientName);
        });
    }
}