using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profiles;

public class SearchProfilesRequest : ClientGetRequest<string, IList<SearchProfileDto>>
{
    public SearchProfilesRequest(string requestData) : base(requestData) { }
    public override string BuildQueryString() => WithOneQueryParameter(new("searchText", RequestData));
}