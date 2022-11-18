namespace PhlegmaticOne.InnoGotchi.Api.Services;

public interface IServerAddressProvider
{
    public Uri ServerAddressUri { get; }
}