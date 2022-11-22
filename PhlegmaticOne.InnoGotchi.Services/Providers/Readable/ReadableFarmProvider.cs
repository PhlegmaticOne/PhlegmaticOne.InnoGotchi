using AutoMapper;
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

    public async Task<OperationResult<int>> GetPetsCountInFarmAsync(Guid farmId)
    {
        var repository = _unitOfWork.GetDataRepository<Farm>();
        var result = await repository.GetByIdOrDefaultAsync(farmId,
            selector: s => s.InnoGotchies.Count);
        return OperationResult.FromSuccess(result);
    }
}