using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class ConstructorController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ConstructorController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        const string initialDirectory = "Resources";
        var initialPath = Path.Combine(_webHostEnvironment.WebRootPath, initialDirectory);
        var componentDirectories = new DirectoryInfo(initialPath);
        var componentPaths = new Dictionary<string, List<string>>();

        foreach (var directory in componentDirectories.EnumerateDirectories())
        {
            var filePaths = directory
                .EnumerateFiles()
                .Select(file => initialDirectory + "/" + directory.Name + "/" + file.Name)
                .ToList();
            componentPaths.Add(directory.Name, filePaths);
        }

        return View(new ConstructorViewModel
        {
            ComponentPaths = componentPaths
        });
    }
}