using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
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
    private readonly IUserProfilesDataService _userProfilesDataService;
    private readonly IFarmsDataService _farmDataService;
    private readonly IMapper _mapper;

    public FarmController(IUserProfilesDataService userProfilesDataService, 
        IFarmsDataService farmDataService,
        IMapper mapper)
    {
        _userProfilesDataService = userProfilesDataService;
        _farmDataService = farmDataService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<OperationResult<FarmDto>> Create([FromBody] CreateFarmDto createFarmDto)
    {
        var userEmail = User.GetUserEmail();

        if (await _farmDataService.ExistsForUserAsync(userEmail))
        {
            var alreadyExistsMessage = $"User {userEmail} already has a farm";
            return OperationResult.FromFail<FarmDto>(customMessage: alreadyExistsMessage);
        }

        var userProfile = await _userProfilesDataService.GetProfileByEmailAsync(userEmail);

        var farm = CreateFarmForProfile(userProfile!, createFarmDto.Name);

        var createdFarm = await _farmDataService.CreateAsync(farm);

        var mapped = _mapper.Map<FarmDto>(createdFarm);

        return OperationResult.FromSuccess(mapped);
    }

    [HttpGet]
    public async Task<OperationResult<FarmDto>> Get()
    {
        var user = User.GetUserEmail();
        var farm = await _farmDataService.GetByEmailAsync(user);

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