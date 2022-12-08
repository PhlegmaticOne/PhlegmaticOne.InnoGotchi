namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Services.Synchronization;

public class ShouldSynchronizePetsProvider : IShouldSynchronizePetsProvider
{
    public ShouldSynchronizePetsProvider(bool shouldSynchronize) => ShouldSynchronize = shouldSynchronize;

    public bool ShouldSynchronize { get; }
}