using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Constructor;

public class CreateInnoGotchiRequest : ClientPostRequest<CreateInnoGotchiDto, PreviewInnoGotchiDto>
{
    public CreateInnoGotchiRequest(CreateInnoGotchiDto requestData) : base(requestData) { }
}