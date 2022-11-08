using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Services.Storage;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers.Base;

public class AuthenticationControllerBase : Controller
{
    private const string Home = "/";
    protected readonly ILocalStorageService LocalStorageService;

    public AuthenticationControllerBase(ILocalStorageService localStorageService) => 
        LocalStorageService = localStorageService;

    protected IActionResult HandleUnauthorizedResponse()
    {
        var loginUrl = LocalStorageService.GetLoginUrl();
        LocalStorageService.SetIsAuthenticationRequired(true);
        LocalStorageService.SetJwtToken(string.Empty);
        return LocalRedirect(loginUrl ?? Home);
    }

    protected string? GetJwtToken() => LocalStorageService.GetJwtToken();
}