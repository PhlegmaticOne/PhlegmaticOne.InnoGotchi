using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms;

public class GetIsFarmExistsQuery : IdentityOperationResultQueryBase<bool>
{
    public GetIsFarmExistsQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetIsFarmExistsQueryHandler : IOperationResultQueryHandler<GetIsFarmExistsQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetIsFarmExistsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<bool>> Handle(GetIsFarmExistsQuery request,
        CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Farm>();
        var isExists = await repository.ExistsAsync(x => x.OwnerId == request.ProfileId, cancellationToken);
        return OperationResult.FromSuccess(isExists);
    }
}