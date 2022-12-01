using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Search;

public class SearchProfilesRequest : IdentityOperationResultQueryBase<IList<SearchProfileDto>>
{
    public string SearchText { get; }

    public SearchProfilesRequest(Guid profileId, string searchText) : base(profileId) => 
        SearchText = searchText;
}

public class SearchProfilesRequestHandler : IOperationResultQueryHandler<SearchProfilesRequest, IList<SearchProfileDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchProfilesRequestHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OperationResult<IList<SearchProfileDto>>> Handle(SearchProfilesRequest request, CancellationToken cancellationToken)
    {
        var toSearch = request.SearchText;
        var profileId = request.ProfileId;

        var repository = _unitOfWork.GetRepository<UserProfile>();
        var result = await repository.GetFirstOrDefaultAsync(
            predicate: p => p.User.Email.Contains(toSearch) &&
                            p.Farm != null &&
                            p.Id != profileId,
            selector: s => new SearchProfileDto
            {
                Email = s.User.Email,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Id = s.Id,
                IsAlreadyCollaborated = s.Collaborations.Select(x => x.Farm.OwnerId).Contains(profileId)
            }, cancellationToken: cancellationToken);


        IList<SearchProfileDto> list = new List<SearchProfileDto>();

        if (result is not null)
        {
            list.Add(result);
        }

        return OperationResult.FromSuccess(list);
    }
}