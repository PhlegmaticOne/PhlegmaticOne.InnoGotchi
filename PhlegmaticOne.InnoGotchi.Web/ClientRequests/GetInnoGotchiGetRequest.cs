using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class GetInnoGotchiGetRequest : ClientGetRequest<Guid, DetailedInnoGotchiDto>
{
    public GetInnoGotchiGetRequest(Guid requestData) : base(requestData) { }

    public override string BuildQueryString() => WithOneQueryParameter(new("petId", RequestData));
}