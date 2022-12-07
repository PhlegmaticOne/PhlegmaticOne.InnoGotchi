using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms.Base;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms;

public class GetFarmByIdQuery : IdentityOperationResultQuery<DetailedFarmDto>
{
    public GetFarmByIdQuery(Guid profileId, Guid farmId) : base(profileId)
    {
        FarmId = farmId;
    }

    public Guid FarmId { get; }
}

public class GetFarmByIdQueryHandler : GetFarmQueryHandlerBase<GetFarmByIdQuery>
{
    private readonly IValidator<GetFarmByIdQuery> _getFarmValidator;

    public GetFarmByIdQueryHandler(IUnitOfWork unitOfWork,
        IValidator<GetFarmByIdQuery> getFarmValidator,
        IMapper mapper) :
        base(unitOfWork, mapper)
    {
        _getFarmValidator = getFarmValidator;
    }

    protected override Expression<Func<Farm, bool>> GetQueryPredicate(GetFarmByIdQuery request)
    {
        return p => p.Id == request.FarmId;
    }

    protected override Task<ValidationResult> ValidateAsync(GetFarmByIdQuery request, CancellationToken cancellationToken)
    {
        return _getFarmValidator.ValidateAsync(request, cancellationToken);
    }
}