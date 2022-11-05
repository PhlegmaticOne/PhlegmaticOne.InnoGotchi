using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SvgController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SvgController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public string Get()
        {
            var serverName = HttpContext.Request.PathBase;
            return serverName;
        }
    }
}