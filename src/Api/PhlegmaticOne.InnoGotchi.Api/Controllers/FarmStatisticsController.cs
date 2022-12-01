using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Services.Queries.FarmStatistics;
using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FarmStatisticsController : IdentityController
{
    private readonly IMediator _mediator;

    public FarmStatisticsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public Task<OperationResult<PreviewFarmStatisticsDto>> GetAuthorized() =>
        _mediator.Send(new GetPreviewStatisticsQuery(ProfileId()), HttpContext.RequestAborted);

    [HttpGet]
    public Task<OperationResult<IList<PreviewFarmStatisticsDto>>> GetCollaborated() =>
        _mediator.Send(new GetCollaboratedFarmStatisticsQuery(ProfileId()), HttpContext.RequestAborted);

    [HttpGet]
    public Task<OperationResult<DetailedFarmStatisticsDto>> GetDetailed() =>
        _mediator.Send(new GetDetailedStatisticsQuery(ProfileId()), HttpContext.RequestAborted);
}