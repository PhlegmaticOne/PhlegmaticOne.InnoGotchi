using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;
using PhlegmaticOne.InnoGotchi.Services.Queries.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<OperationResult<AuthorizedProfileDto>> Register([FromBody] RegisterProfileDto registerProfileDto)
    {
        var registerOperationResult =
            await _mediator.Send(new RegisterProfileCommand(registerProfileDto), HttpContext.RequestAborted);

        if (registerOperationResult.IsSuccess == false)
            return OperationResult.Failed<AuthorizedProfileDto>(registerOperationResult.ErrorMessage);

        return await AuthorizeAsync(registerProfileDto.Email, registerProfileDto.Password);
    }

    [HttpPost]
    public Task<OperationResult<AuthorizedProfileDto>> Login([FromBody] LoginDto loginDto)
    {
        return AuthorizeAsync(loginDto.Email, loginDto.Password);
    }

    private Task<OperationResult<AuthorizedProfileDto>> AuthorizeAsync(string email, string password)
    {
        return _mediator.Send(new GetAuthorizedProfileAnonymousQuery(email, password), HttpContext.RequestAborted);
    }
}