namespace PhlegmaticOne.InnoGotchi.Api.Services;

public class ServerAddressProvider : IServerAddressProvider
{
    public ServerAddressProvider(string serverAddress) => 
        ServerAddressUri = new Uri(serverAddress);

    public Uri ServerAddressUri { get; }
}