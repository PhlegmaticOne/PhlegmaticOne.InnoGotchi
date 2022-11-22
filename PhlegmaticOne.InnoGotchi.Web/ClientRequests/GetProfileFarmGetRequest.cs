using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class GetProfileFarmGetRequest : ClientGetRequest<Guid, DetailedFarmDto>
{
    public GetProfileFarmGetRequest(Guid requestData) : base(requestData) { }

    public override string BuildQueryString() => WithOneQueryParameter(new("profileId", RequestData));
}