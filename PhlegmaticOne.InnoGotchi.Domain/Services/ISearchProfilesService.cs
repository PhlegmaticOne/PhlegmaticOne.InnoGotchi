using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Services;

public interface ISearchProfilesService
{
    Task<IList<UserProfile>> SearchProfilesAsync(string searchText);
}