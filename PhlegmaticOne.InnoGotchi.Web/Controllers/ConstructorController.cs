using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Extentions;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.LocalStorage.Extensions;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class ConstructorController : ClientRequestsControllerBase
{
    public ConstructorController(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService) : 
        base(clientRequestsService, localStorageService) { }

    [HttpGet]
    public async Task<IActionResult> Build()
    {
        var serverResponse =
            await SendAuthorizedGetRequestAsync<InnoGotchiComponentCollectionDto>(new GetAllInnoGotchiComponentsRequest());

        if (serverResponse.IsUnauthorized)
        {
            return HandleUnauthorizedResponse();
        }

        var data = serverResponse.GetData<InnoGotchiComponentCollectionDto>();

        return View(new ConstructorViewModel
        {
            ComponentsByCategories = GetComponentsByCategories(data)
        });
    }

    private IEnumerable<IGrouping<string, string>> GetComponentsByCategories(InnoGotchiComponentCollectionDto data)
    {
        var serverAddress = LocalStorageService.GetServerAddress();
        return data.Components.GroupBy(x => x.Name,
                s => serverAddress.Combine(s.ImageUrl).ToString());
    }
}