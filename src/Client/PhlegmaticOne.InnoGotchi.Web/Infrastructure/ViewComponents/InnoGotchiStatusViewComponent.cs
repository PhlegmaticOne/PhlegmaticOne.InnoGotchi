using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.ViewComponents.Base;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.ViewComponents;

public class InnoGotchiStatusViewComponent : ViewComponent
{
    private readonly IInnoGotchiStatusGenerator _innoGotchiStatusGenerator;

    public InnoGotchiStatusViewComponent(IInnoGotchiStatusGenerator innoGotchiStatusGenerator) => 
        _innoGotchiStatusGenerator = innoGotchiStatusGenerator;

    public IViewComponentResult Invoke(string status)
    {
        var cssClasses = _innoGotchiStatusGenerator.GenerateBadgeCssClasses(status);
        var model = new InnoGotchiStatusViewModel
        {
            BadgeStyleClasses = cssClasses,
            Status = status
        };
        return View("~/Views/_Partial_Views/InnoGotchies/InnoGotchiStatus.cshtml", model);
    }
}