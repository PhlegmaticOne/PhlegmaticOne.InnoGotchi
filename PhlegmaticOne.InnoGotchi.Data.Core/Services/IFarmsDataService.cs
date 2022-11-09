using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Core.Services;

public interface IFarmsDataService
{
    Task<bool> ExistsForUserAsync(string email);
    Task<Farm> CreateAsync(Farm farm);
    Task<Farm?> GetByEmailAsync(string email);
}