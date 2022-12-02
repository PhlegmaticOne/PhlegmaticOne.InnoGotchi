using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Services.Synchronization;
using PhlegmaticOne.InnoGotchi.Services.Commands.Farms;
using PhlegmaticOne.InnoGotchi.Services.Commands.Synchronization;
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
    private readonly IShouldSynchronizePetsProvider _shouldSynchronizePetsProvider;

    public FarmsController(IMediator mediator,
        IShouldSynchronizePetsProvider shouldSynchronizePetsProvider)
    {
        _mediator = mediator;
        _shouldSynchronizePetsProvider = shouldSynchronizePetsProvider;
    }

    [HttpGet]
    public async Task<OperationResult<DetailedFarmDto>> GetAuthorized()
    {
        if (_shouldSynchronizePetsProvider.ShouldSynchronize)
            await _mediator.Send(new SynchronizeInnoGotchiesInProfileFarmCommand(ProfileId()),
                HttpContext.RequestAborted);

        return await _mediator
            .Send(new GetFarmByProfileQuery(ProfileId()), HttpContext.RequestAborted);
    }

    [HttpGet]
    public async Task<OperationResult<DetailedFarmDto>> Get(Guid farmId)
    {
        if (_shouldSynchronizePetsProvider.ShouldSynchronize)
            await _mediator.Send(new SynchronizeInnoGotchiesInFarmCommand(farmId),
                HttpContext.RequestAborted);

        return await _mediator
            .Send(new GetFarmByIdQuery(ProfileId(), farmId), HttpContext.RequestAborted);
    }

    [HttpGet]
    public Task<OperationResult<IList<PreviewFarmDto>>> GetCollaborated()
    {
        return _mediator.Send(new GetCollaboratedFarmsQuery(ProfileId()), HttpContext.RequestAborted);
    }

    [HttpGet]
    public Task<OperationResult<bool>> Exists()
    {
        return _mediator.Send(new GetIsFarmExistsQuery(ProfileId()), HttpContext.RequestAborted);
    }

    [HttpPost]
    public Task<OperationResult> Create([FromBody] CreateFarmDto createFarmDto)
    {
        return _mediator.Send(new CreateFarmCommand(ProfileId(), createFarmDto), HttpContext.RequestAborted);
    }
}