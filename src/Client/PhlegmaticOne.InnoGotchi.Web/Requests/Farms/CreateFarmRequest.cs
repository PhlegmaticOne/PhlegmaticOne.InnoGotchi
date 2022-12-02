using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Farms;

public class CreateFarmRequest : ClientOperationResultPostRequest<CreateFarmDto>
{
    public CreateFarmRequest(CreateFarmDto requestData) : base(requestData)
    {
    }
}