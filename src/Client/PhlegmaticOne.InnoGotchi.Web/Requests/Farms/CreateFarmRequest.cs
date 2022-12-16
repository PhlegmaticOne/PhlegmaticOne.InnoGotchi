using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Farms;

public class CreateFarmRequest : ClientOperationResultPostRequest<CreateFarmDto>
{
    public CreateFarmRequest(CreateFarmDto requestData) : base(requestData)
    {
    }
}