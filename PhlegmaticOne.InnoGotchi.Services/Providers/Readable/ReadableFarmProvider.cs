﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableFarmProvider : IReadableFarmProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadableFarmProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OperationResult<Farm>> GetFarmAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<Farm>();
        var farm = await repository.GetFirstOrDefaultAsync(x => x.Owner.Id == profileId);
        return OperationResult.FromSuccess(farm)!;
    }

    public async Task<OperationResult<Farm>> GetFarmWithProfileAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<Farm>();
        var farm = await repository.GetFirstOrDefaultAsync(x => x.Owner.Id == profileId,
            include: i => i.Include(x => x.Owner).ThenInclude(x => x.User));
        return OperationResult.FromSuccess(farm)!;
    }

    public async Task<OperationResult<int>> GetPetsCountInFarmAsync(Guid farmId)
    {
        var repository = _unitOfWork.GetDataRepository<InnoGotchiModel>();
        var count = await repository.CountAsync(x => x.Farm.Id == farmId);
        return OperationResult.FromSuccess(count);
    }
}