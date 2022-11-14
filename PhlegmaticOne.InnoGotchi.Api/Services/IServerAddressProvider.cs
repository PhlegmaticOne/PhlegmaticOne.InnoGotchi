namespace PhlegmaticOne.InnoGotchi.Api.Services;

public interface IServerAddressProvider
{
    public string ServerAddress { get; }
    public Uri ServerAddressUri { get; }
}

public class ServerAddressProvider : IServerAddressProvider
{
    public ServerAddressProvider(string serverAddress)
    {
        ServerAddress = serverAddress;
        ServerAddressUri = new Uri(serverAddress);
    }

    public string ServerAddress { get; }
    public Uri ServerAddressUri { get; }
}