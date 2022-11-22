using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PagedLists;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IInnoGotchiManager
{
    Task<OperationResult<DetailedInnoGotchiDto>> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto);
    Task<OperationResult<DetailedInnoGotchiDto>> GetDetailedAsync(IdentityModel<IdDto> petIdModel);
    Task<OperationResult<PagedList<PreviewInnoGotchiDto>>> GetPagedAsync(Guid profileId, int pageIndex);
}