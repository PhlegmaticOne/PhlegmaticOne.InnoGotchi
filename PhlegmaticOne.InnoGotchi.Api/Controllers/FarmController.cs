using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.Verifying.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FarmController : DataController
{
    private readonly IVerifyingService<IdentityFarmModel, Farm> _farmVerifyingService;

    public FarmController(IDataService dataService, IMapper mapper,
        IVerifyingService<IdentityFarmModel, Farm> farmVerifyingService) : 
        base(dataService, mapper)
    {
        _farmVerifyingService = farmVerifyingService;
    }

    [HttpGet]
    public async Task<OperationResult<DetailedFarmDto>> Get()
    {
        var userId = ProfileId();
        var farmDataService = DataService.GetDataRepository<Farm>();
        var farm = await farmDataService.GetFirstOrDefaultAsync(x => x.Owner.Id == userId,
            include: i => i.Include(x => x.InnoGotchies).ThenInclude(x => x.Components).ThenInclude(x => x.InnoGotchiComponent));

        if (farm is null)
        {
            var notExistsMessage = $"There is not farm created for user: {User.GetUserEmail()}";
            return OperationResult.FromFail<DetailedFarmDto>(notExistsMessage);
        }

        return ResultFromMap<DetailedFarmDto>(farm);
    }

    [HttpPost]
    public async Task<OperationResult<DetailedFarmDto>> Create([FromBody] CreateFarmDto createFarmDto)
    {
        var profileFarmModel = Mapper.MapIdentity<IdentityFarmModel>(createFarmDto, ProfileId());
        var validationResult = await _farmVerifyingService.ValidateAsync(profileFarmModel);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedFarmDto>(validationResult.ToDictionary());
        }

        var newFarm = await _farmVerifyingService.MapAsync(profileFarmModel);
        return await MapFromInsertionResult<DetailedFarmDto, Farm>(newFarm);
    }
}