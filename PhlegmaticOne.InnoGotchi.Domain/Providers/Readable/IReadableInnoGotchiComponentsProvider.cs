using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableInnoGotchiComponentsProvider
{
    Task<IList<InnoGotchiComponent>> GetAllComponentsAsync();
}