using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class CreateCollaborationRequest : ClientPostRequest<IdDto, CollaborationDto>
{
    public CreateCollaborationRequest(IdDto requestData) : base(requestData) { }
}