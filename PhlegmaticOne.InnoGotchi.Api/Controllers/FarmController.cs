using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FarmController : DataController
{
    private readonly IVerifyingService<IdentityInnoGotchiModel, InnoGotchiModel> _innoGotchiVerifyingService;
    private readonly IVerifyingService<IdentityFarmModel, Farm> _farmVerifyingService;

    public FarmController(IDataService dataService, IMapper mapper,
        IVerifyingService<IdentityFarmModel, Farm> farmVerifyingService,
        IVerifyingService<IdentityInnoGotchiModel, InnoGotchiModel> innoGotchiVerifyingService) : 
        base(dataService, mapper)
    {
        _farmVerifyingService = farmVerifyingService;
        _innoGotchiVerifyingService = innoGotchiVerifyingService;
    }

    [HttpGet]
    public async Task<OperationResult<FarmDto>> Get()
    {
        var userId = UserId();
        var farmDataService = DataService.GetDataRepository<Farm>();
        var farm = await farmDataService.GetFirstOrDefaultAsync(x => x.Owner.Id == userId);

        if (farm is null)
        {
            var notExistsMessage = $"There is not farm created for user: {User.GetUserEmail()}";
            return OperationResult.FromFail<FarmDto>(notExistsMessage);
        }

        return ResultFromMap<FarmDto>(farm);
    }

    [HttpPost]
    public async Task<OperationResult<FarmDto>> Create([FromBody] CreateFarmDto createFarmDto)
    {
        var profileFarmModel = Mapper.MapIdentity<IdentityFarmModel>(createFarmDto, UserId());
        var validationResult = await _farmVerifyingService.ValidateAsync(profileFarmModel);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<FarmDto>(validationResult.ToString());
        }

        var newFarm = await _farmVerifyingService.MapAsync(profileFarmModel);
        return await MapFromInsertionResult<FarmDto, Farm>(newFarm);
    }

    [HttpPost]
    public async Task<OperationResult<InnoGotchiDto>> Add([FromBody] CreateInnoGotchiDto createInnoGotchiDto)
    {
        var profileInnoGotchiModel = Mapper.MapIdentity<IdentityInnoGotchiModel>(createInnoGotchiDto, UserId());
        var validationResult = await _innoGotchiVerifyingService.ValidateAsync(profileInnoGotchiModel);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<InnoGotchiDto>(validationResult.ToString());
        }

        var created = await _innoGotchiVerifyingService.MapAsync(profileInnoGotchiModel);
        return await MapFromInsertionResult<InnoGotchiDto, InnoGotchiModel>(created);
    }
}