using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms.Base;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.UnitOfWork.Interfaces;
using System.Linq.Expressions;
using PhlegmaticOne.OperationResults.Mediatr;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms;

public class GetFarmByIdQuery : IdentityOperationResultQueryBase<DetailedFarmDto>
{
    public Guid FarmId { get; }

    public GetFarmByIdQuery(Guid profileId, Guid farmId) : base(profileId) => 
        FarmId = farmId;
}

public class GetFarmByIdQueryHandler : GetFarmQueryHandlerBase<GetFarmByIdQuery>
{
    private readonly IValidator<GetFarmByIdQuery> _getFarmValidator;

    public GetFarmByIdQueryHandler(IUnitOfWork unitOfWork,
        IValidator<GetFarmByIdQuery> getFarmValidator,
        IMapper mapper) :
        base(unitOfWork, mapper) =>
        _getFarmValidator = getFarmValidator;

    protected override Expression<Func<Farm, bool>> GetQueryPredicate(GetFarmByIdQuery request) =>
        p => p.Id == request.FarmId;

    protected override Task<ValidationResult> ValidateAsync(GetFarmByIdQuery request) =>
        _getFarmValidator.ValidateAsync(request);
}