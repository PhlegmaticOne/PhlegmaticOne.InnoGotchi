using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchiComponents;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class InnoGotchiComponentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InnoGotchiComponentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public Task<OperationResult<IList<InnoGotchiComponentDto>>> GetAll() =>
        _mediator.Send(new GetAllComponentsQuery(), HttpContext.RequestAborted);
}