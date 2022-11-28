using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Collaborations;

public class CreateCollaborationRequest : ClientPostRequest<CreateCollaborationDto, CollaborationDto>
{
    public CreateCollaborationRequest(CreateCollaborationDto requestData) : base(requestData) { }
}