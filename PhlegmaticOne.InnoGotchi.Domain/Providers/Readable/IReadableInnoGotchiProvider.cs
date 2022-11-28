using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.PagedLists;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableInnoGotchiProvider
{
    Task<InnoGotchiModel> GetDetailedAsync(Guid petId, Guid profileId);
    Task<IList<InnoGotchiModel>> GetAllDetailedAsync(Guid farmId);
    Task<IList<InnoGotchiModel>> GetAllAsync(Guid farmId);
    Task<PagedList<InnoGotchiModel>> GetPagedAsync(Guid profileId, PagedListData pagedListData);
}