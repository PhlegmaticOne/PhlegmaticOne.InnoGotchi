using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Constructor;

public class GetAllInnoGotchiComponentsRequest : EmptyClientGetRequest<IList<InnoGotchiComponentDto>>
{
}