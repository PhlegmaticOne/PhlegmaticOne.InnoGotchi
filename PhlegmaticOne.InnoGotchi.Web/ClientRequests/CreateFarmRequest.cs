using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class CreateFarmRequest : ClientPostRequest<CreateFarmDto, FarmDto>
{
    public CreateFarmRequest(CreateFarmDto requestData) : base(requestData) { }
}