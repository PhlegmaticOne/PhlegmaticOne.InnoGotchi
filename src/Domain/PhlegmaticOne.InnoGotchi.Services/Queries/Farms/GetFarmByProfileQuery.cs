using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms.Base;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms;

public class GetFarmByProfileQuery : IdentityOperationResultQuery<DetailedFarmDto>
{
    public GetFarmByProfileQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetFarmByProfileQueryHandler : GetFarmQueryHandlerBase<GetFarmByProfileQuery>
{
    private readonly IValidator<ProfileFarmModel> _farmValidator;

    public GetFarmByProfileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IValidator<ProfileFarmModel> farmValidator) :
        base(unitOfWork, mapper)
    {
        _farmValidator = farmValidator;
    }

    protected override Expression<Func<Farm, bool>> GetQueryPredicate(GetFarmByProfileQuery request)
    {
        return p => p.OwnerId == request.ProfileId;
    }

    protected override Task<ValidationResult> ValidateAsync(GetFarmByProfileQuery request, CancellationToken cancellationToken)
    {
        return _farmValidator.ValidateAsync(new ProfileFarmModel(request.ProfileId), cancellationToken);
    }
}