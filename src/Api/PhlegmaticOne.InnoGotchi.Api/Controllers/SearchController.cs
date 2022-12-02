using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Services.Queries.Search;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[AllowAnonymous]
public class SearchController : IdentityController
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<OperationResult<IList<SearchProfileDto>>> Profiles(string searchText)
    {
        return _mediator.Send(new SearchProfilesRequest(ProfileId(), searchText), HttpContext.RequestAborted);
    }
}