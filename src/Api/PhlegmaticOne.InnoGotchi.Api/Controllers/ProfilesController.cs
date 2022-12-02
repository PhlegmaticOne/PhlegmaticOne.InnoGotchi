using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;
using PhlegmaticOne.InnoGotchi.Services.Queries.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ProfilesController : IdentityController
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<OperationResult<DetailedProfileDto>> GetAuthorized()
    {
        return _mediator.Send(new GetDetailedProfileQuery(ProfileId()), HttpContext.RequestAborted);
    }

    [HttpPut]
    public async Task<OperationResult<AuthorizedProfileDto>> Update([FromBody] UpdateProfileDto updateProfileDto)
    {
        var updateResult = await _mediator.Send(new UpdateProfileCommand(ProfileId(), updateProfileDto),
            HttpContext.RequestAborted);

        if (updateResult.IsSuccess == false)
            return OperationResult.FromFail<AuthorizedProfileDto>(updateResult.ErrorMessage);

        return await _mediator.Send(new GetAuthorizedProfileAuthorizedQuery(ProfileId()),
            HttpContext.RequestAborted);
    }
}