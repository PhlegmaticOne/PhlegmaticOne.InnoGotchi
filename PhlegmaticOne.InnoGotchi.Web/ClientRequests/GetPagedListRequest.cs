using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.PagedLists;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class GetPagedListRequest : ClientGetRequest<PagedListData, PagedList<ReadonlyInnoGotchiPreviewDto>>
{
    public GetPagedListRequest(PagedListData requestData) : base(requestData) { }

    public override string BuildQueryString() =>
        WithManyQueryParameters(
            new("pageIndex", RequestData.PageIndex),
            new("pageSize", RequestData.PageSize),
            new("sortType", RequestData.SortType),
            new("isAscending", RequestData.IsAscending));
}