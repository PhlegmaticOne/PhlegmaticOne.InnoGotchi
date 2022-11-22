using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.PagedLists;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class GetPagedListRequest : ClientGetRequest<int, PagedList<PreviewInnoGotchiDto>>
{
    public GetPagedListRequest(int requestData) : base(requestData) { }

    public override string BuildQueryString() => WithOneQueryParameter(new("pageIndex", RequestData));
}