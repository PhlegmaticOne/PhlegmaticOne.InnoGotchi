using PhlegmaticOne.ApiRequesting.Models;
using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Farms;

public class GetFarmRequest : ClientGetRequest<Guid, DetailedFarmDto>
{
    public GetFarmRequest(Guid requestData) : base(requestData)
    {
    }

    public override string BuildQueryString()
    {
        return WithOneQueryParameter(new GetRequestQueryParameter("farmId", RequestData));
    }
}