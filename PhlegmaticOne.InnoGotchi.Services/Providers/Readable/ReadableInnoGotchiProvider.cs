using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableInnoGotchiProvider : IReadableInnoGotchiProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadableInnoGotchiProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OperationResult<InnoGotchiModel>> GetDetailedAsync(Guid petId, Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<InnoGotchiModel>();
        var pet = await repository.GetByIdOrDefaultAsync(petId,
            include: IncludeComponents());

        return OperationResult.FromSuccess(pet)!;
    }

    public async Task<OperationResult<IList<InnoGotchiModel>>> GetAllDetailedAsync(Guid farmId)
    {
        var repository = _unitOfWork.GetDataRepository<InnoGotchiModel>();
        var pets = await repository.GetAllAsync(
            include: IncludeComponents(),
            predicate: p => p.Farm.Id == farmId);

        return OperationResult.FromSuccess(pets);
    }

    public async Task<OperationResult<IList<InnoGotchiModel>>> GetAllAsync(Guid farmId)
    {
        var repository = _unitOfWork.GetDataRepository<InnoGotchiModel>();
        var pets = await repository.GetAllAsync(predicate: p => p.Farm.Id == farmId);
        return OperationResult.FromSuccess(pets);
    }


    private static Func<IQueryable<InnoGotchiModel>, IIncludableQueryable<InnoGotchiModel, object>> IncludeComponents() =>
        i => i.Include(x => x.Components).ThenInclude(x => x.InnoGotchiComponent);
    private static Expression<Func<InnoGotchiModel, bool>> WhereProfileIdIs(Guid profileId) =>
        x => x.Farm.OwnerId == profileId;
}