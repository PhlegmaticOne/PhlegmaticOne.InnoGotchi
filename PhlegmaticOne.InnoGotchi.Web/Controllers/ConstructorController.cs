using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Extentions;
using PhlegmaticOne.InnoGotchi.Web.Services.Storage;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class ConstructorController : AuthenticationControllerBase
{
    private readonly IClientRequestsService _clientRequestsService;

    public ConstructorController(IClientRequestsService clientRequestsService, 
        ILocalStorageService localStorageService) : base(localStorageService) =>
        _clientRequestsService = clientRequestsService;


    [HttpGet]
    public async Task<IActionResult> Build()
    {
        var jwtToken = GetJwtToken();
        var serverAddress = LocalStorageService.GetServerAddress();

        var serverResult = await _clientRequestsService
            .GetAsync<InnoGotchiComponentCollectionDto>(new GetAllInnoGotchiComponentsRequest(), jwtToken);

        if (serverResult.IsUnauthorized)
        {
            return HandleUnauthorizedResponse();
        }

        var response = serverResult.GetData<InnoGotchiComponentCollectionDto>();

        var result = response.Components
            .GroupBy(x => x.Name,
                s => serverAddress.Combine(s.ImageUrl).ToString());

        return View(new ConstructorViewModel
        {
            ComponentsByCategories = result
        });
    }
}