using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Models;
using System.Diagnostics;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //var path1 = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "body1.svg");
            //var path2 = Path.Combine(_webHostEnvironment.WebRootPath, "Resources", "eyes1.svg");
            //var image1 = System.IO.File.ReadAllBytes(path1);
            //var image2 = System.IO.File.ReadAllBytes(path2);
            //ViewBag.Image1 = image1;
            //ViewBag.Image2 = image2;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}