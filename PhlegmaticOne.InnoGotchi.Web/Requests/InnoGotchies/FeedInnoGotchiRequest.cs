using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class FeedInnoGotchiRequest : ClientPutRequest<IdDto, DetailedInnoGotchiDto>
{
    public FeedInnoGotchiRequest(IdDto requestData) : base(requestData) { }
}