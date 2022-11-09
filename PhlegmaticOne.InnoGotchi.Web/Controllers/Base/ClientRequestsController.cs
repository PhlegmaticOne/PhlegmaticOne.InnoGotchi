using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Extentions;
using PhlegmaticOne.InnoGotchi.Web.Helpers;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.ServerRequesting.Models;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers.Base;

public class ClientRequestsController : Controller
{
    protected readonly ILocalStorageService LocalStorageService;
    protected readonly IClientRequestsService ClientRequestsService;

    public ClientRequestsController(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService)
    {
        LocalStorageService = localStorageService;
        ClientRequestsService = clientRequestsService;
    }

    protected async Task<ServerResponse<OperationResult<TResponse>>> SendAuthorizedGetRequestAsync<TResponse>(ClientGetRequest clientGetRequest)
    {
        var jwtToken = GetJwtToken();
        var serverResponse = await ClientRequestsService.GetAsync<TResponse>(clientGetRequest, jwtToken);
        await HandleResponse(serverResponse);
        return serverResponse;
    }

    protected async Task<ServerResponse<OperationResult<TResponse>>> SendAuthorizedPostRequestAsync<TResponse>(ClientPostRequest clientPostRequest)
    {
        var jwtToken = GetJwtToken();
        var serverResponse = await ClientRequestsService.PostAsync<TResponse>(clientPostRequest, jwtToken);
        await HandleResponse(serverResponse);
        return serverResponse;
    }

    protected IActionResult LoginRedirect()
    {
        var loginPath = LocalStorageService.GetLoginPath();
        return LocalRedirect(loginPath ?? Constants.HomeUrl);
    }

    protected async Task SignOutAsync()
    {
        SetJwtToken(string.Empty);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    protected async Task SignInAsync(ClaimsPrincipal claimsPrincipal, string jwtToken)
    {
        SetJwtToken(jwtToken);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
    }

    protected string? GetJwtToken() => LocalStorageService.GetJwtToken();
    protected void SetJwtToken(string jwtToken) => LocalStorageService.SetJwtToken(jwtToken);

    private async Task HandleResponse(ServerResponse serverResponse)
    {
        if(serverResponse.IsUnauthorized == false) return;

        await SignOutAsync();
    }
}