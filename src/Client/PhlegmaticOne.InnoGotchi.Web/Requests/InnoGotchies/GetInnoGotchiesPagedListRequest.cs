using PhlegmaticOne.ApiRequesting.Models;
using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.PagedLists.Implementation;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class GetInnoGotchiesPagedListRequest : ClientGetRequest<PagedListData, PagedList<ReadonlyInnoGotchiPreviewDto>>
{
    public GetInnoGotchiesPagedListRequest(PagedListData requestData) : base(requestData)
    {
    }

    public override string BuildQueryString()
    {
        return WithManyQueryParameters(
            new GetRequestQueryParameter("pageIndex", RequestData.PageIndex),
            new GetRequestQueryParameter("pageSize", RequestData.PageSize),
            new GetRequestQueryParameter("sortType", RequestData.SortType),
            new GetRequestQueryParameter("isAscending", RequestData.IsAscending));
    }
}