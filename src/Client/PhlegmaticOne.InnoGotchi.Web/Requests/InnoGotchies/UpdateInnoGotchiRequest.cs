using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class UpdateInnoGotchiRequest : ClientOperationResultPutRequest<UpdateInnoGotchiDto>
{
    public UpdateInnoGotchiRequest(UpdateInnoGotchiDto requestData) : base(requestData) { }
}