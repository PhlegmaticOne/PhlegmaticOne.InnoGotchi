using PhlegmaticOne.ApiRequesting.Models;
using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profiles;

public class SearchProfilesRequest : ClientGetRequest<string, IList<SearchProfileDto>>
{
    public SearchProfilesRequest(string requestData) : base(requestData)
    {
    }

    public override string BuildQueryString()
    {
        return WithOneQueryParameter(new GetRequestQueryParameter("searchText", RequestData));
    }
}