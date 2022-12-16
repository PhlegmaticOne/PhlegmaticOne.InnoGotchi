using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Collaborations;

public class CreateCollaborationRequest : ClientOperationResultPostRequest<CreateCollaborationDto>
{
    public CreateCollaborationRequest(CreateCollaborationDto requestData) : base(requestData)
    {
    }
}