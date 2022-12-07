using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Statistics;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Statistics;

public class GetCollaboratedFarmStatisticsQuery : IdentityOperationResultQuery<IList<PreviewFarmStatisticsDto>>
{
    public GetCollaboratedFarmStatisticsQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetCollaboratedFarmStatisticsQueryHandler :
    IOperationResultQueryHandler<GetCollaboratedFarmStatisticsQuery, IList<PreviewFarmStatisticsDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCollaboratedFarmStatisticsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<IList<PreviewFarmStatisticsDto>>> Handle(
        GetCollaboratedFarmStatisticsQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Collaboration>();

        var result = await repository.GetAllAsync(
            predicate: p => p.UserProfileId == request.ProfileId,
            selector: s => new PreviewFarmStatisticsDto
            {
                FarmId = s.FarmId,
                FarmName = s.Farm.Name,
                ProfileLastName = s.Farm.Owner.LastName,
                ProfileFirstName = s.Farm.Owner.FirstName,
                ProfileEmail = s.Farm.Owner.User.Email,
                PetsCount = s.Farm.InnoGotchies.Count
            }, cancellationToken: cancellationToken);

        return OperationResult.Successful(result);
    }
}