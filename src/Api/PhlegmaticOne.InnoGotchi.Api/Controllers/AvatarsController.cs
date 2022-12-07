using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Services.Queries.Avatars;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class AvatarsController : IdentityController
{
    private readonly IMediator _mediator;

    public AvatarsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<OperationResult<byte[]>> GetAuthorized()
    {
        var queryResult = await _mediator.Send(new GetProfileAvatarQuery(ProfileId()),
            HttpContext.RequestAborted);

        return queryResult.IsSuccess == false
            ? OperationResult.Failed<byte[]>(queryResult.ErrorMessage)
            : OperationResult.Successful(queryResult.Result!.AvatarData);
    }
}