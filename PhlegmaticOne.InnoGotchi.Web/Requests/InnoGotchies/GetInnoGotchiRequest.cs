using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class GetInnoGotchiRequest : ClientGetRequest<Guid, DetailedInnoGotchiDto>
{
    public GetInnoGotchiRequest(Guid requestData) : base(requestData) { }

    public override string BuildQueryString() => WithOneQueryParameter(new("petId", RequestData));
}