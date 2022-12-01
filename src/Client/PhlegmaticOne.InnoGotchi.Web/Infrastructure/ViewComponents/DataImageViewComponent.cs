using Microsoft.AspNetCore.Mvc;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.ViewComponents;

public class DataImageViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(byte[] imageData, int width)
    {
        ViewData["Width"] = width;
        return View("~/Views/_Partial_Views/Shared/DataImage.cshtml", imageData);
    }
}