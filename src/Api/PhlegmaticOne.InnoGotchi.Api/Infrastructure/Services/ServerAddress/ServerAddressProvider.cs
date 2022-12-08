namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Services.ServerAddress;

public class ServerAddressProvider : IServerAddressProvider
{
    public ServerAddressProvider(string serverAddress) => ServerAddressUri = new Uri(serverAddress);

    public Uri ServerAddressUri { get; }
}