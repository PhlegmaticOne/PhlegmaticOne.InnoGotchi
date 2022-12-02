using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Collaborations;

public class CreateCollaborationRequest : ClientOperationResultPostRequest<CreateCollaborationDto>
{
    public CreateCollaborationRequest(CreateCollaborationDto requestData) : base(requestData)
    {
    }
}