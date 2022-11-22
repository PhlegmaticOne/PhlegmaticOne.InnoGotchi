using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Services;

public class SearchProfilesService : ISearchProfilesService
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchProfilesService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task<IList<UserProfile>> SearchProfilesAsync(string searchText)
    {
        var repository = _unitOfWork.GetDataRepository<UserProfile>();
        return repository.GetAllAsync(
            predicate: p => p.User.Email.Contains(searchText));
    }
}