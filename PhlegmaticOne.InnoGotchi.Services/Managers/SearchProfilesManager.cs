using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class SearchProfilesManager : ISearchProfilesManager
{
    public Task<OperationResult<IList<SearchProfileDto>>> SearchAsync(string searchText)
    {
        throw new NotImplementedException();
    }
}