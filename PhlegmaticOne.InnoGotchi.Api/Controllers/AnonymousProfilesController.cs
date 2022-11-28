using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[AllowAnonymous]
public class AnonymousProfilesController : ControllerBase
{
    private readonly IProfileAnonymousActionsManager _profileActionsManager;

    public AnonymousProfilesController(IProfileAnonymousActionsManager profileActionsManager) => 
        _profileActionsManager = profileActionsManager;

    [HttpPost]
    public Task<OperationResult<AuthorizedProfileDto>> Register([FromBody] RegisterProfileDto registerProfileDto) => 
        _profileActionsManager.RegisterAsync(registerProfileDto);

    [HttpPost]
    public Task<OperationResult<AuthorizedProfileDto>> Login([FromBody] LoginDto loginDto) => 
        _profileActionsManager.LoginAsync(loginDto);
}