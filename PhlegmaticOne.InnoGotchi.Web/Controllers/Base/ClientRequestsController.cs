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
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Base;

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
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null)
    {
        var serverResponse = await ClientRequestsService.GetAsync(clientGetRequest, JwtToken());
        return await HandleResponse(serverResponse, onSuccess, onOperationFailed, onServerResponseFailed);
    }

    protected async Task<IActionResult> FromAuthorizedPost<TRequest, TResponse>(
        ClientPostRequest<TRequest, TResponse> clientPostRequest,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null)
    {
        var serverResponse = await ClientRequestsService.PostAsync(clientPostRequest, JwtToken());
        return await HandleResponse(serverResponse, onSuccess, onOperationFailed, onServerResponseFailed);
    }

    protected async Task<IActionResult> FromAuthorizedPut<TRequest, TResponse>(
        ClientPutRequest<TRequest, TResponse> clientPostRequest,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null)
    {
        var serverResponse = await ClientRequestsService.PutAsync(clientPostRequest, JwtToken());
        return await HandleResponse(serverResponse, onSuccess, onOperationFailed, onServerResponseFailed);
    }

    protected IActionResult LoginView() => 
        Redirect(LocalStorageService.GetLoginPath() ?? Constants.HomeUrl);

    protected IActionResult ErrorView(string errorMessage) => 
        RedirectToAction("Error", "Home", new { errorMessage = errorMessage });

    protected IActionResult HomeView() => Redirect(Constants.HomeUrl);

    protected IActionResult ViewWithErrorsFromOperationResult(OperationResult operationResult, string viewName, ErrorHavingViewModel viewModel)
    {
        operationResult.AddErrorsToModelState(ModelState);
        viewModel.ErrorMessage = operationResult.ErrorMessage;
        return View(viewName, viewModel);
    }

    protected IActionResult ViewWithErrorsFromValidationResult(ValidationResult validationResult, string viewName, ErrorHavingViewModel viewModel)
    {
        validationResult.AddToModelState(ModelState);
        return View(viewName, viewModel);
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

    protected string? JwtToken() => LocalStorageService.GetJwtToken();
    protected void SetJwtToken(string jwtToken) => LocalStorageService.SetJwtToken(jwtToken);

    private async Task<IActionResult> HandleResponse<TResponse>(
        ServerResponse<TResponse> serverResponse,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null)
    {
        if (serverResponse.IsUnauthorized)
        {
            await SignOutAsync();
            return LoginView();
        }

        if (serverResponse.IsSuccess == false)
        {
            return onServerResponseFailed is not null ?
                onServerResponseFailed(serverResponse) :
                ErrorView(serverResponse.ToString());
        }

        var operationResult = serverResponse.OperationResult!;

        if (operationResult.IsSuccess == false)
        {
            return onOperationFailed is not null ? 
                onOperationFailed(operationResult) :
                ErrorView(operationResult.ErrorMessage!);
        }

        var data = serverResponse.GetData()!;
        return await onSuccess(data);
    }
}