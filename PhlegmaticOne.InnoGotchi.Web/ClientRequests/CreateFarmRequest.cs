using PhlegmaticOne.InnoGotchi.Shared.Dtos.Farms;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class CreateFarmRequest : ClientPostRequest<CreateFarmDto>
{
    public CreateFarmRequest(CreateFarmDto requestData) : base(requestData) { }
}