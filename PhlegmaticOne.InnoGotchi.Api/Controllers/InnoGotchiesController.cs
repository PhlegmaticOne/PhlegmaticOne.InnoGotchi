using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PagedLists.Implementation;

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

    [HttpGet]
    public Task<OperationResult<DetailedInnoGotchiDto>> GetDetailed(Guid petId) => 
        _innoGotchiesManager.GetDetailedAsync(IdentityModel(new InnoGotchiRequestDto(petId)));

    [HttpGet]
    public Task<OperationResult<PreviewInnoGotchiDto>> GetPreview(Guid petId) =>
        _innoGotchiesManager.GetPreviewAsync(petId);

    [HttpGet]
    public Task<OperationResult<PagedList<ReadonlyInnoGotchiPreviewDto>>> GetPaged(
        [FromQuery] PagedListData pagedListData) => 
        _innoGotchiesManager.GetPagedAsync(IdentityModel(pagedListData));

    [HttpPost]
    public Task<OperationResult> Create([FromBody] CreateInnoGotchiDto createInnoGotchiDto) =>
        _innoGotchiesManager.CreateAsync(IdentityModel(createInnoGotchiDto));

    [HttpPut]
    public Task<OperationResult> Update([FromBody] UpdateInnoGotchiDto updateInnoGotchiDto) =>
        _innoGotchiActionsManager.UpdateAsync(IdentityModel(updateInnoGotchiDto));
}