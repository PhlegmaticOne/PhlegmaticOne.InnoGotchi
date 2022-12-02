using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class GetDetailedInnoGotchiRequest : ClientGetRequest<Guid, DetailedInnoGotchiDto>
{
    public GetDetailedInnoGotchiRequest(Guid requestData) : base(requestData)
    {
    }

    public override string BuildQueryString()
    {
        return WithOneQueryParameter(new GetRequestQueryParameter("petId", RequestData));
    }
}