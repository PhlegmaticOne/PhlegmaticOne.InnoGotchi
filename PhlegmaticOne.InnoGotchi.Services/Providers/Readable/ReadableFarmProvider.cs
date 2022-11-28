using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableFarmProvider : IReadableFarmProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadableFarmProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task<Farm> GetFarmAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetRepository<Farm>();
        return repository.GetFirstOrDefaultAsync(x => x.Owner.Id == profileId)!;
    }

    public Task<Farm> GetFarmWithProfileAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetRepository<Farm>();
        return repository.GetFirstOrDefaultAsync(x => x.Owner.Id == profileId,
            include: i => i.Include(x => x.Owner).ThenInclude(x => x.User))!;
    }

    public Task<int> GetPetsCountInFarmAsync(Guid farmId)
    {
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        return repository.CountAsync(x => x.Farm.Id == farmId);
    }
}