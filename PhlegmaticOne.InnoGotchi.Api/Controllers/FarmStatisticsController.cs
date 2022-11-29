using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FarmStatisticsController : IdentityController
{
    private readonly IFarmStatisticsManager _farmStatisticsManager;

    public FarmStatisticsController(IFarmStatisticsManager farmStatisticsManager) => 
        _farmStatisticsManager = farmStatisticsManager;

    [HttpGet]
    public Task<OperationResult<PreviewFarmStatisticsDto>> GetAuthorized() => 
        _farmStatisticsManager.BuildForProfileAsync(ProfileId());

    [HttpGet]
    public Task<OperationResult<IList<PreviewFarmStatisticsDto>>> GetCollaborated() => 
        _farmStatisticsManager.BuildForCollaboratedProfilesAsync(ProfileId());

    [HttpGet]
    public Task<OperationResult<DetailedFarmStatisticsDto>> GetDetailed() => 
        _farmStatisticsManager.BuildDetailedForProfileAsync(ProfileId());
}