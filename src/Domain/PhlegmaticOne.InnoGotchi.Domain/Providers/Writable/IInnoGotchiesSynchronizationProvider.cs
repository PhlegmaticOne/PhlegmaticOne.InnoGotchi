namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IInnoGotchiesSynchronizationProvider
{
    Task SynchronizeAllPetsAsync(CancellationToken cancellationToken = new());
    Task SynchronizePetAsync(Guid petId, CancellationToken cancellationToken = new());
    Task SynchronizePetsInFarmAsync(Guid farmId, CancellationToken cancellationToken = new());
}