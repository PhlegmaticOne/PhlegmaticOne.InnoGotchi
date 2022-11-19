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
    public Task<OperationResult<DetailedFarmDto>> Get() => 
        _farmManager.GetWithPetsAsync(ProfileId());

    [HttpPost]
    public Task<OperationResult<DetailedFarmDto>> Create([FromBody] CreateFarmDto createFarmDto) => 
        _farmManager.CreateAsync(IdentityModel(createFarmDto));
}