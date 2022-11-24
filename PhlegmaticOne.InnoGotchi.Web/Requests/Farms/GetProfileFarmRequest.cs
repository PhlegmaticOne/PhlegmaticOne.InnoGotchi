using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Farms;

public class GetProfileFarmRequest : ClientGetRequest<Guid, DetailedFarmDto>
{
    public GetProfileFarmRequest(Guid requestData) : base(requestData) { }

    public override string BuildQueryString() => WithOneQueryParameter(new("profileId", RequestData));
}