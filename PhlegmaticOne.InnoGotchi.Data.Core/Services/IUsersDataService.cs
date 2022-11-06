namespace PhlegmaticOne.InnoGotchi.Data.Core.Services;

public interface IUsersDataService
{
    Task<bool> IsExistsAsync(string email, string passwordHash);
}