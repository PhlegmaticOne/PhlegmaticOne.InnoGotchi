using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Core.Services;

public interface IInnoGotchiComponentsDataService
{
    Task<IEnumerable<InnoGotchiComponent>> GetAllAsync();
}