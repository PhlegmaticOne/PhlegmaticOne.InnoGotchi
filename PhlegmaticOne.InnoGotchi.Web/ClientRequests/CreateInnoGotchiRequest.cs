using PhlegmaticOne.InnoGotchi.Shared.Dtos.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class CreateInnoGotchiRequest : ClientPostRequest<CreateInnoGotchiDto, InnoGotchiDto>
{
    public CreateInnoGotchiRequest(CreateInnoGotchiDto requestData) : base(requestData) { }
}