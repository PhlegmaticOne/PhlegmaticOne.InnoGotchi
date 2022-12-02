namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IInnoGotchiOwnChecker
{
    Task<bool> IsBelongAsync(Guid profileId, Guid petId, CancellationToken cancellationToken = new());
}