using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Services.Commands.Collaborations;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class CollaborationsController : IdentityController
{
    private readonly IMediator _mediator;

    public CollaborationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public Task<OperationResult> Create([FromBody] CreateCollaborationDto createCollaborationDto)
    {
        var fromProfileId = ProfileId();
        var toProfileId = createCollaborationDto.ToProfileId;
        return _mediator.Send(new CreateCollaborationCommand(fromProfileId, toProfileId), HttpContext.RequestAborted);
    }
}