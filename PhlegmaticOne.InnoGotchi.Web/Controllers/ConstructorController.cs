using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;
using PhlegmaticOne.InnoGotchi.Shared.OperationResults;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Extentions;
using PhlegmaticOne.InnoGotchi.Web.Services.Storage;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class ConstructorController : Controller
{
    private readonly IClientRequestsService _clientRequestsService;
    private readonly ILocalStorageService _localStorageService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ConstructorController(IClientRequestsService clientRequestsService, 
        ILocalStorageService localStorageService,
        IWebHostEnvironment webHostEnvironment)
    {
        _clientRequestsService = clientRequestsService;
        _localStorageService = localStorageService;
        _webHostEnvironment = webHostEnvironment;
    }


    [HttpGet]
    public async Task<IActionResult> Build()
    {
        var jwtToken = _localStorageService.GetJwtToken();
        var serverAddress = _localStorageService.GetServerAddress();
        var serverResult = await _clientRequestsService
            .GetAsync<OperationResult<InnoGotchiComponentCollectionDto>>(new GetAllInnoGotchiComponentsRequest(), jwtToken);

        if (serverResult.IsUnauthorized)
        {
            return Redirect("~/Account/Login");
        }

        var response = serverResult.ResponseData!.Result!;

        var result = response.Components
            .GroupBy(x => x.Name,
                s => serverAddress.Combine(s.ImageUrl).ToString());

        return View(new ConstructorViewModel
        {
            ComponentsByCategories = result
        });
    }
}