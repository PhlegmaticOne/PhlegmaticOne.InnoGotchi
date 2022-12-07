using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Queries.Profiles.Base;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Profiles;

public class GetAuthorizedProfileAuthorizedQuery : IdentityOperationResultQuery<AuthorizedProfileDto>
{
    public GetAuthorizedProfileAuthorizedQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetAuthorizedProfileAuthorizedQueryHandler :
    GetAuthorizedProfileQueryHandlerBase<GetAuthorizedProfileAuthorizedQuery>
{
    public GetAuthorizedProfileAuthorizedQueryHandler(IUnitOfWork unitOfWork,
        IJwtTokenGenerationService jwtTokenGenerationService) :
        base(unitOfWork, jwtTokenGenerationService)
    {
    }

    protected override Task<AuthorizedProfileDto?> GetAuthorizedProfileAsync(
        GetAuthorizedProfileAuthorizedQuery request,
        CancellationToken cancellationToken)
    {
        var repository = UnitOfWork.GetRepository<UserProfile>();
        return repository.GetByIdOrDefaultAsync(request.ProfileId,
            x => new AuthorizedProfileDto
            {
                Email = x.User.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Id = x.Id
            }, cancellationToken: cancellationToken);
    }
}