using Microsoft.AspNetCore.Mvc;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.ViewComponents;

public class BaseStringImageViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string imageData, int width)
    {
        ViewData["Width"] = width;
        return View("~/Views/_Partial_Views/Shared/BaseStringImage.cshtml", imageData);
    }
}