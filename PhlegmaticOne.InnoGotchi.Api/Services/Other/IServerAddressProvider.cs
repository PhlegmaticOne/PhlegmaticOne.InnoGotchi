namespace PhlegmaticOne.InnoGotchi.Api.Services.Other;

public interface IServerAddressProvider
{
    public string ServerAddress { get; }
    public Uri ServerAddressUri { get; }
}