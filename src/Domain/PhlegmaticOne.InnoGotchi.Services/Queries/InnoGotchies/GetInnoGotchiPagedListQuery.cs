using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PagedLists.Implementation;
using PhlegmaticOne.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchies;

public class GetInnoGotchiPagedListQuery : IdentityOperationResultQuery<PagedList<ReadonlyInnoGotchiPreviewDto>>
{
    public GetInnoGotchiPagedListQuery(Guid profileId, PagedListData pagedListData) : base(profileId)
    {
        PagedListData = pagedListData;
    }

    public PagedListData PagedListData { get; }
}

public class GetInnoGotchiPagedListQueryHandler :
    IOperationResultQueryHandler<GetInnoGotchiPagedListQuery, PagedList<ReadonlyInnoGotchiPreviewDto>>
{
    private readonly IMapper _mapper;
    private readonly ISortingService<InnoGotchiModel> _sortingService;
    private readonly IUnitOfWork _unitOfWork;

    public GetInnoGotchiPagedListQueryHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ISortingService<InnoGotchiModel> sortingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sortingService = sortingService;
    }

    public async Task<OperationResult<PagedList<ReadonlyInnoGotchiPreviewDto>>> Handle(
        GetInnoGotchiPagedListQuery request, CancellationToken cancellationToken)
    {
        var pagedListData = request.PagedListData;
        var profileId = request.ProfileId;

        var sortingFunc = _sortingService.GetSortingFunc(pagedListData.SortType, pagedListData.IsAscending);

        var result = await _unitOfWork
            .GetRepository<InnoGotchiModel>()
            .GetPagedListAsync(
                pageSize: pagedListData.PageSize,
                pageIndex: pagedListData.PageIndex,
                orderBy: sortingFunc,
                predicate: WherePetsNotInFarmWithOwner(profileId),
                include: IncludeWithProfile(), cancellationToken: cancellationToken);

        var mapped = _mapper.Map<PagedList<ReadonlyInnoGotchiPreviewDto>>(result);
        return OperationResult.Successful(mapped);
    }

    private static Expression<Func<InnoGotchiModel, bool>> WherePetsNotInFarmWithOwner(Guid ownerId)
    {
        return p => p.Farm.Owner.Id != ownerId;
    }

    private static Func<IQueryable<InnoGotchiModel>, IIncludableQueryable<InnoGotchiModel, object>> IncludeWithProfile()
    {
        return i => i
            .Include(x => x.Components)
            .ThenInclude(x => x.InnoGotchiComponent)
            .Include(x => x.Farm)
            .ThenInclude(x => x.Owner);
    }
}