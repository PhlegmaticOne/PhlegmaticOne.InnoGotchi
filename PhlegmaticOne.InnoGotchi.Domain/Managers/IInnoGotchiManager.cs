using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PagedLists.Implementation;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IInnoGotchiManager
{
    Task<OperationResult> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto);
    Task<OperationResult<DetailedInnoGotchiDto>> GetDetailedAsync(IdentityModel<InnoGotchiRequestDto> petIdModel);
    Task<OperationResult<PreviewInnoGotchiDto>> GetPreviewAsync(Guid petId);
    Task<OperationResult<PagedList<ReadonlyInnoGotchiPreviewDto>>> GetPagedAsync(IdentityModel<PagedListData> pagedIdentityModel);
}