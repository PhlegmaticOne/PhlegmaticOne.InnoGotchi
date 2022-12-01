using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PagedLists.Implementation;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class InnoGotchiesController : IdentityController
{
    private readonly IMediator _mediator;

    public InnoGotchiesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public Task<OperationResult<DetailedInnoGotchiDto>> GetDetailed(Guid petId) =>
        _mediator.Send(new GetDetailedInnoGotchiQuery(ProfileId(), petId), HttpContext.RequestAborted);

    [HttpGet]
    public Task<OperationResult<PreviewInnoGotchiDto>> GetPreview(Guid petId) =>
        _mediator.Send(new GetPreviewInnoGotchiQuery(petId), HttpContext.RequestAborted);

    [HttpGet]
    public Task<OperationResult<PagedList<ReadonlyInnoGotchiPreviewDto>>> GetPaged(
        [FromQuery] PagedListData pagedListData) =>
        _mediator.Send(new GetInnoGotchiPagedListQuery(ProfileId(), pagedListData),
            HttpContext.RequestAborted);

    [HttpPost]
    public Task<OperationResult> Create([FromBody] CreateInnoGotchiDto createInnoGotchiDto) =>
        _mediator.Send(new CreateInnoGotchiCommand(ProfileId(), createInnoGotchiDto),
            HttpContext.RequestAborted);

    [HttpPut]
    public Task<OperationResult> Update([FromBody] UpdateInnoGotchiDto updateInnoGotchiDto) =>
        _mediator.Send(new UpdateInnoGotchiCommand(ProfileId(), updateInnoGotchiDto),
            HttpContext.RequestAborted);
}