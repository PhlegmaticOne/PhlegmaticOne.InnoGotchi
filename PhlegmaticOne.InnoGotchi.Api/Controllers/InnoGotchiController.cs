using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InnoGotchiController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public InnoGotchiController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Authorize]
    public string Get()
    {
        return "Innogotchi";
    }
}