using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Services.Commands.Farms;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FarmsController : IdentityController
{
    private readonly IMediator _mediator;

    public FarmsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public Task<OperationResult<DetailedFarmDto>> GetAuthorized() =>
        _mediator.Send(new GetFarmByProfileQuery(ProfileId()), HttpContext.RequestAborted);

    [HttpGet]
    public Task<OperationResult<DetailedFarmDto>> Get(Guid farmId) =>
        _mediator.Send(new GetFarmByIdQuery(ProfileId(), farmId), HttpContext.RequestAborted);

    [HttpGet]
    public Task<OperationResult<IList<PreviewFarmDto>>> GetCollaborated() =>
        _mediator.Send(new GetCollaboratedFarmsQuery(ProfileId()), HttpContext.RequestAborted);

    [HttpGet]
    public Task<OperationResult<bool>> Exists() =>
        _mediator.Send(new GetIsFarmExistsQuery(ProfileId()), HttpContext.RequestAborted);

    [HttpPost]
    public Task<OperationResult> Create([FromBody] CreateFarmDto createFarmDto) =>
        _mediator.Send(new CreateFarmCommand(ProfileId(), createFarmDto), HttpContext.RequestAborted);
}