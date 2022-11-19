using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class InnoGotchiesController : IdentityController
{
    private readonly IInnoGotchiManager _innoGotchiesManager;
    private readonly IInnoGotchiActionsManager _innoGotchiActionsManager;

    public InnoGotchiesController(IInnoGotchiManager innoGotchiesManager,
        IInnoGotchiActionsManager innoGotchiActionsManager)
    {
        _innoGotchiesManager = innoGotchiesManager;
        _innoGotchiActionsManager = innoGotchiActionsManager;
    }

    [HttpPost]
    public Task<OperationResult<DetailedInnoGotchiDto>> Create([FromBody] CreateInnoGotchiDto createInnoGotchiDto)
    {
        var identityModel = IdentityModel(createInnoGotchiDto);
        return _innoGotchiesManager.CreateAsync(identityModel);
    }

    [HttpGet]
    public Task<OperationResult<DetailedInnoGotchiDto>> Get(Guid petId)
    {
        var identityModel = IdentityModel(new IdDto { Id = petId });
        return _innoGotchiesManager.GetDetailedAsync(identityModel);
    }

    [HttpPut]
    public async Task<OperationResult<DetailedInnoGotchiDto>> Drink([FromBody] IdDto idDto)
    {
        var identityModel = IdentityModel(idDto);
        var drinkResult = await _innoGotchiActionsManager.DrinkAsync(identityModel);

        if (drinkResult.IsSuccess == false)
        {
            OperationResult.FromFail<DetailedInnoGotchiDto>(drinkResult.ErrorMessage);
        }

        var updated = await _innoGotchiesManager.GetDetailedAsync(identityModel);
        return OperationResult.FromSuccess(updated.Result!);
    }

    [HttpPut]
    public async Task<OperationResult<DetailedInnoGotchiDto>> Feed([FromBody] IdDto idDto)
    {
        var identityModel = IdentityModel(idDto);
        var feedResult = await _innoGotchiActionsManager.FeedAsync(identityModel);

        if (feedResult.IsSuccess == false)
        {
            OperationResult.FromFail<DetailedInnoGotchiDto>(feedResult.ErrorMessage);
        }

        var updated = await _innoGotchiesManager.GetDetailedAsync(identityModel);
        return OperationResult.FromSuccess(updated.Result!);
    }
}