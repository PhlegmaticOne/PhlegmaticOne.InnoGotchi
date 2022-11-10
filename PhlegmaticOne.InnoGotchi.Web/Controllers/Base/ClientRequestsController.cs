using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Extentions;
using PhlegmaticOne.InnoGotchi.Web.Helpers;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.ServerRequesting.Models;
using PhlegmaticOne.ServerRequesting.Services;
using System.Security.Claims;

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

    protected async Task<IActionResult> FromAuthorizedGet<TRequest, TResponse>(
        ClientGetRequest<TRequest, TResponse> clientGetRequest,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null,
        Func<ServerResponse, IActionResult>? onUnauthorized = null)
    {
        var jwtToken = GetJwtToken();
        var serverResponse = await ClientRequestsService.GetAsync(clientGetRequest, jwtToken);
        return await HandleResponse(serverResponse, onSuccess,
            onOperationFailed, onServerResponseFailed, onUnauthorized);
    }

    protected async Task<IActionResult> FromAuthorizedPost<TRequest, TResponse>(
        ClientPostRequest<TRequest, TResponse> clientPostRequest,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null,
        Func<ServerResponse, IActionResult>? onUnauthorized = null)
    {
        var jwtToken = GetJwtToken();
        var serverResponse = await ClientRequestsService.PostAsync(clientPostRequest, jwtToken);
        return await HandleResponse(serverResponse, onSuccess,
            onOperationFailed, onServerResponseFailed, onUnauthorized);
    }

    protected IActionResult LoginRedirect()
    {
        var loginPath = LocalStorageService.GetLoginPath();
        return LocalRedirect(loginPath ?? Constants.HomeUrl);
    }

    protected IActionResult ErrorRedirect()
    {
        var errorPath = LocalStorageService.GetErrorPath();
        return LocalRedirect(errorPath ?? Constants.HomeUrl);
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

    private async Task<IActionResult> HandleResponse<TResponse>(ServerResponse<TResponse> serverResponse,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null,
        Func<ServerResponse, IActionResult>? onUnauthorized = null)
    {
        if (serverResponse.IsUnauthorized)
        {
            await SignOutAsync();
            return onUnauthorized is not null ? onUnauthorized(serverResponse) : LoginRedirect();
        }

        if (serverResponse.IsSuccess == false)
        {
            return onServerResponseFailed is not null ? onServerResponseFailed(serverResponse) : ErrorRedirect();
        }

        var operationResult = serverResponse.OperationResult!;

        if (operationResult.IsSuccess == false)
        {
            return onOperationFailed is not null ? onOperationFailed(operationResult) : ErrorRedirect();
        }

        var data = serverResponse.GetData()!;
        return await onSuccess(data);
    }
}