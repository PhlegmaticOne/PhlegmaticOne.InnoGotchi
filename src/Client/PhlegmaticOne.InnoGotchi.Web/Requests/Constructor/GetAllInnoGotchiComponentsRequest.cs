using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Constructor;

public class GetAllInnoGotchiComponentsRequest : EmptyClientGetRequest<IList<InnoGotchiComponentDto>> { }