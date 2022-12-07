using System.Linq.Expressions;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms.Base;

public abstract class GetFarmQueryHandlerBase<TRequest> : IOperationResultQueryHandler<TRequest, DetailedFarmDto>
    where TRequest : IOperationResultQuery<DetailedFarmDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    protected GetFarmQueryHandlerBase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult<DetailedFarmDto>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return OperationResult.Failed<DetailedFarmDto>(validationResult.ToString());
        }

        var result = await _unitOfWork.GetRepository<Farm>().GetFirstOrDefaultAsync(
            GetQueryPredicate(request), i => i
                .Include(x => x.Owner)
                .ThenInclude(x => x.User)
                .Include(x => x.InnoGotchies)
                .ThenInclude(x => x.Components)
                .ThenInclude(x => x.InnoGotchiComponent), cancellationToken);

        var mapped = _mapper.Map<DetailedFarmDto>(result);
        return OperationResult.Successful(mapped);
    }

    protected abstract Expression<Func<Farm, bool>> GetQueryPredicate(TRequest request);
    protected abstract Task<ValidationResult> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}