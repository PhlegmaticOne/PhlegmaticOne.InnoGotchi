using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface ISearchProfilesManager
{
    Task<OperationResult<IList<SearchProfileDto>>> SearchAsync(Guid profileId, string searchText);
}