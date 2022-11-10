using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Farms;
using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FarmController : ControllerBase
{
    private readonly IDataRepository<UserProfile> _userProfilesDataService;
    private readonly IDataRepository<Farm> _farmDataService;
    private readonly IMapper _mapper;

    public FarmController(IDataService dataService,
        IMapper mapper)
    {
        _userProfilesDataService = dataService.GetDataRepository<UserProfile>();
        _farmDataService = dataService.GetDataRepository<Farm>();
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<OperationResult<FarmDto>> Create([FromBody] CreateFarmDto createFarmDto)
    {
        var userId = User.GetUserId();

        if (await _farmDataService.ExistsAsync(x => x.Owner.Id == userId))
        {
            var alreadyExistsMessage = $"User {userId} already has a farm";
            return OperationResult.FromFail<FarmDto>(customMessage: alreadyExistsMessage);
        }

        var userProfile = await _userProfilesDataService.GetByIdOrDefaultAsync(userId);

        var farm = CreateFarmForProfile(userProfile!, createFarmDto.Name);

        var createdFarm = await _farmDataService.CreateAsync(farm);

        var mapped = _mapper.Map<FarmDto>(createdFarm);

        return OperationResult.FromSuccess(mapped);
    }

    [HttpGet]
    public async Task<OperationResult<FarmDto>> Get()
    {
        var userId = User.GetUserId();
        var farm = await _farmDataService.GetFirstOrDefaultAsync(x => x.Owner.Id == userId);

        if (farm is null)
        {
            return OperationResult.FromFail<FarmDto>();
        }

        var mapped = _mapper.Map<FarmDto>(farm);

        return OperationResult.FromSuccess(mapped);
    }

    private static Farm CreateFarmForProfile(UserProfile userProfile, string name) =>
        new()
        {
            Name = name,
            Owner = userProfile
        };
}