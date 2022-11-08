using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.LocalStorage.Extensions;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.ServerRequesting.Models;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers.Base;

public class ClientRequestsControllerBase : Controller
{
    private const string Home = "/";

    protected readonly ILocalStorageService LocalStorageService;
    protected readonly IClientRequestsService ClientRequestsService;

    public ClientRequestsControllerBase(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService)
    {
        LocalStorageService = localStorageService;
        ClientRequestsService = clientRequestsService;
    }

    protected IActionResult HandleUnauthorizedResponse()
    {
        var loginUrl = LocalStorageService.GetLoginUrl();
        LocalStorageService.SetIsAuthenticationRequired(true);
        LocalStorageService.SetJwtToken(string.Empty);
        return LocalRedirect(loginUrl ?? Home);
    }

    protected async Task<ServerResponse<OperationResult<TResponse>>> SendAuthorizedGetRequestAsync<TResponse>(ClientGetRequest clientGetRequest)
    {
        var jwtToken = GetJwtToken();
        var serverResult = await ClientRequestsService
            .GetAsync<TResponse>(clientGetRequest, jwtToken);
        return serverResult;
    }

    protected string? GetJwtToken() => LocalStorageService.GetJwtToken();
}