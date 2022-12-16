using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Constructor;

public class CreateInnoGotchiRequest : ClientOperationResultPostRequest<CreateInnoGotchiDto>
{
    public CreateInnoGotchiRequest(CreateInnoGotchiDto requestData) : base(requestData)
    {
    }
}