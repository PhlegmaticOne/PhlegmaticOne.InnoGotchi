using System.Linq.Expressions;
using AutoMapper;
using FluentValidation.Results;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms.Base;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms;

public class GetFarmByProfileQuery : IdentityOperationResultQueryBase<DetailedFarmDto>
{
    public GetFarmByProfileQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetFarmByProfileQueryHandler : GetFarmQueryHandlerBase<GetFarmByProfileQuery>
{
    public GetFarmByProfileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) :
        base(unitOfWork, mapper)
    {
    }

    protected override Expression<Func<Farm, bool>> GetQueryPredicate(GetFarmByProfileQuery request)
    {
        return p => p.OwnerId == request.ProfileId;
    }

    protected override Task<ValidationResult> ValidateAsync(GetFarmByProfileQuery request)
    {
        return Task.FromResult(new ValidationResult());
    }
}