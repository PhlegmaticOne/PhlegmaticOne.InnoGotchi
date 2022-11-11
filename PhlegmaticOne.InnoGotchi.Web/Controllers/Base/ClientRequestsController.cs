using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.ServerRequesting.Models;
using PhlegmaticOne.ServerRequesting.Services;
using System.Security.Claims;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Helpers;

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

    protected IActionResult ToLoginView()
    {
        var loginPath = LocalStorageService.GetLoginPath();
        return Redirect(loginPath ?? Constants.HomeUrl);
    }

    protected IActionResult ToErrorView()
    {
        var errorPath = LocalStorageService.GetErrorPath();
        return Redirect(errorPath ?? Constants.HomeUrl);
    }

    protected IActionResult ToHomeView() => Redirect(Constants.HomeUrl);

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

    protected IActionResult AddErrorsAndReturnView(ValidationResult validationResult, string viewName, object model)
    {
        validationResult.AddToModelState(ModelState);
        return View(viewName, model);
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
            return onUnauthorized is not null ? onUnauthorized(serverResponse) : ToLoginView();
        }

        if (serverResponse.IsSuccess == false)
        {
            return onServerResponseFailed is not null ? onServerResponseFailed(serverResponse) : ToErrorView();
        }

        var operationResult = serverResponse.OperationResult!;

        if (operationResult.IsSuccess == false)
        {
            return onOperationFailed is not null ? onOperationFailed(operationResult) : ToErrorView();
        }

        var data = serverResponse.GetData()!;
        return await onSuccess(data);
    }
}