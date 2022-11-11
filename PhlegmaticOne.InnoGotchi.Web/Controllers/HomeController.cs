using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Other;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}