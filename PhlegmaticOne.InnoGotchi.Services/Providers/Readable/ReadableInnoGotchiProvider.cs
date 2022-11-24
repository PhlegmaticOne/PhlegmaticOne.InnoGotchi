using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;
using System.Linq.Expressions;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.PagedLists;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableInnoGotchiProvider : IReadableInnoGotchiProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISortingService<InnoGotchiModel> _sortingService;
    public ReadableInnoGotchiProvider(IUnitOfWork unitOfWork, ISortingService<InnoGotchiModel> sortingService)
    {
        _unitOfWork = unitOfWork;
        _sortingService = sortingService;
    }

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

    public async Task<OperationResult<PagedList<InnoGotchiModel>>> GetPagedAsync(Guid profileId, PagedListData pagedListData)
    {
        var sortingFunc = _sortingService.GetSortingFunc(pagedListData.SortType, pagedListData.IsAscending);

        var result = await _unitOfWork
            .GetDataRepository<InnoGotchiModel>()
            .GetPagedListAsync(
                pageSize: pagedListData.PageSize,
                pageIndex: pagedListData.PageIndex,
                orderBy: sortingFunc,
                predicate: WherePetsNotInFarmWithOwner(profileId),
                include: IncludeWithProfile());

        return OperationResult.FromSuccess(result);
    }

    private static Expression<Func<InnoGotchiModel, bool>> WherePetsNotInFarmWithOwner(Guid ownerId) =>
        p => p.Farm.OwnerId != ownerId;
    private static Func<IQueryable<InnoGotchiModel>, IIncludableQueryable<InnoGotchiModel, object>> IncludeComponents() =>
        i => i.Include(x => x.Components).ThenInclude(x => x.InnoGotchiComponent);

    private static Func<IQueryable<InnoGotchiModel>, IIncludableQueryable<InnoGotchiModel, object>> IncludeWithProfile() =>
        i => i
            .Include(x => x.Components)
                .ThenInclude(x => x.InnoGotchiComponent)
            .Include(x => x.Farm)
                .ThenInclude(x => x.Owner);
}