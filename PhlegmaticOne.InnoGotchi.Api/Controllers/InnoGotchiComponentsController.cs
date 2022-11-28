using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class InnoGotchiComponentsController : ControllerBase
{
    private readonly IInnoGotchiComponentsManager _innoGotchiComponentsManager;

    public InnoGotchiComponentsController(IInnoGotchiComponentsManager innoGotchiComponentsManager) => 
        _innoGotchiComponentsManager = innoGotchiComponentsManager;

    [HttpGet]
    public Task<OperationResult<IList<InnoGotchiComponentDto>>> GetAll() => 
        _innoGotchiComponentsManager.GetAllComponentsAsync();
}