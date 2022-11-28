using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class CollaborationsController : IdentityController
{
    private readonly ICollaborationManager _collaborationManager;

    public CollaborationsController(ICollaborationManager collaborationManager) => 
        _collaborationManager = collaborationManager;

    [HttpPost]
    public Task<OperationResult<CollaborationDto>> Create([FromBody] CreateCollaborationDto createCollaborationDto) => 
        _collaborationManager.AddCollaboratorAsync(IdentityModel(createCollaborationDto));
}