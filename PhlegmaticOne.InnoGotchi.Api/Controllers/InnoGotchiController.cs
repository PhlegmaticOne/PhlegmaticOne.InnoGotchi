using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class InnoGotchiController
{
    [HttpGet]
    [Authorize]
    public OperationResult<string> Test(int id)
    {
        return OperationResult<string>.FromSuccess($"inno gotchi: {id}");
    }
}