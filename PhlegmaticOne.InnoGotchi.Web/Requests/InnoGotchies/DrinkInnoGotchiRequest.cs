using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class DrinkInnoGotchiRequest : ClientPutRequest<IdDto, DetailedInnoGotchiDto>
{
    public DrinkInnoGotchiRequest(IdDto requestData) : base(requestData) { }
}