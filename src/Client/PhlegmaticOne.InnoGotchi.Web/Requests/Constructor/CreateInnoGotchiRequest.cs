using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Constructor;

public class CreateInnoGotchiRequest : ClientOperationResultPostRequest<CreateInnoGotchiDto>
{
    public CreateInnoGotchiRequest(CreateInnoGotchiDto requestData) : base(requestData)
    {
    }
}