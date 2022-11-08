using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Core.Services;

public interface IUsersDataService
{
    Task<bool> ExistsAsync(string email);
    Task<User?> GetByEmailAsync(string email);
}