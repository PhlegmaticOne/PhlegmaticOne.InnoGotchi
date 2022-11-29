using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.PagedLists.Implementation;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class GetInnoGotchiesPagedListRequest : ClientGetRequest<PagedListData, PagedList<ReadonlyInnoGotchiPreviewDto>>
{
    public GetInnoGotchiesPagedListRequest(PagedListData requestData) : base(requestData) { }

    public override string BuildQueryString() =>
        WithManyQueryParameters(
            new("pageIndex", RequestData.PageIndex),
            new("pageSize", RequestData.PageSize),
            new("sortType", RequestData.SortType),
            new("isAscending", RequestData.IsAscending));
}