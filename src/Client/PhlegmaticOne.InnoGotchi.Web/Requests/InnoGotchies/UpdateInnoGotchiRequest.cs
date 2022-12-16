using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class UpdateInnoGotchiRequest : ClientOperationResultPutRequest<UpdateInnoGotchiDto>
{
    public UpdateInnoGotchiRequest(UpdateInnoGotchiDto requestData) : base(requestData)
    {
    }
}