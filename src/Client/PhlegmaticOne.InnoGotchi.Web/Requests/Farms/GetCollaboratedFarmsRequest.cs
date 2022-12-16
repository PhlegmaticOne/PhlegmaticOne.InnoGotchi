using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Farms;

public class GetCollaboratedFarmsRequest : EmptyClientGetRequest<IList<PreviewFarmDto>>
{
}