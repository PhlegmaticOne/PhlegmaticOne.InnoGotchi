﻿using AutoMapper;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;
using System.Linq.Expressions;
using PhlegmaticOne.OperationResults.Mediatr;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms.Base;

public abstract class GetFarmQueryHandlerBase<TRequest> : IOperationResultQueryHandler<TRequest, DetailedFarmDto>
    where TRequest : IOperationResultQuery<DetailedFarmDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    protected GetFarmQueryHandlerBase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult<DetailedFarmDto>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateAsync(request);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedFarmDto>(validationResult.ToString());
        }

        var farmsRepository = _unitOfWork.GetRepository<Farm>();

        var result = await farmsRepository.GetFirstOrDefaultAsync(
            predicate: GetQueryPredicate(request),
            include: i => i
                .Include(x => x.Owner)
                .ThenInclude(x => x.User)
                .Include(x => x.InnoGotchies)
                .ThenInclude(x => x.Components)
                .ThenInclude(x => x.InnoGotchiComponent), cancellationToken: cancellationToken);

        if (result is null)
        {
            return OperationResult.FromFail<DetailedFarmDto>("Profile doesn't have a farm");
        }

        var mapped = _mapper.Map<DetailedFarmDto>(result);
        return OperationResult.FromSuccess(mapped);
    }

    protected abstract Expression<Func<Farm, bool>> GetQueryPredicate(TRequest request);
    protected abstract Task<ValidationResult> ValidateAsync(TRequest request);
}