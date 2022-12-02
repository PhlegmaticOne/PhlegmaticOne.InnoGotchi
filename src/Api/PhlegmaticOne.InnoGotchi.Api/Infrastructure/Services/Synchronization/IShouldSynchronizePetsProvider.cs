namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Services.Synchronization;

public interface IShouldSynchronizePetsProvider
{
    bool ShouldSynchronize { get; }
}