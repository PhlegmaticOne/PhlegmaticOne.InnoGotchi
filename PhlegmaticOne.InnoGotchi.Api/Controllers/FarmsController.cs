using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FarmsController : IdentityController
{
    private readonly IFarmManager _farmManager;
    public FarmsController(IFarmManager farmManager) => _farmManager = farmManager;

    [HttpGet]
    public Task<OperationResult<DetailedFarmDto>> GetForAuthorized() => 
        _farmManager.GetWithPetsAsync(ProfileId());

    [HttpGet]
    public Task<OperationResult<DetailedFarmDto>> Get(Guid profileId) =>
        _farmManager.GetWithPetsAsync(profileId);

    [HttpGet]
    public Task<OperationResult<IList<PreviewFarmDto>>> GetCollaboratedFarms() => 
        _farmManager.GetCollaboratedAsync(ProfileId());

    [HttpPost]
    public Task<OperationResult<DetailedFarmDto>> Create([FromBody] CreateFarmDto createFarmDto) => 
        _farmManager.CreateAsync(IdentityModel(createFarmDto));
}