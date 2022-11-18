using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class InnoGotchiesController : IdentityController
{
    private readonly IInnoGotchiesManager _innoGotchiesManager;

    public InnoGotchiesController(IInnoGotchiesManager innoGotchiesManager) => 
        _innoGotchiesManager = innoGotchiesManager;

    [HttpPost]
    public Task<OperationResult<DetailedInnoGotchiDto>> Create([FromBody] CreateInnoGotchiDto createInnoGotchiDto)
    {
        var identityModel = IdentityModel(createInnoGotchiDto);
        return _innoGotchiesManager.CreateAsync(identityModel);
    }

    [HttpGet]
    public Task<OperationResult<DetailedInnoGotchiDto>> Get(Guid petId)
    {
        var identityModel = IdentityModel(petId);
        return _innoGotchiesManager.GetDetailedAsync(identityModel);
    }
}