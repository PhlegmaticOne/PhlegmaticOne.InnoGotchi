using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Extentions;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class ConstructorController : ClientRequestsController
{
    public ConstructorController(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService) :
        base(clientRequestsService, localStorageService)
    { }

    [HttpGet]
    public Task<IActionResult> Build()
    {
        return FromAuthorizedGet(new GetAllInnoGotchiComponentsRequest(), components =>
        {
            IActionResult view = View(new ConstructorViewModel
            {
                ComponentsByCategories = GetComponentsByCategories(components)
            });
            return Task.FromResult(view);
        });
    }

    [HttpPost]
    public string CreateNew([FromBody] CreateInnoGotchiViewModel createInnoGotchiViewModel)
    {
        var result = createInnoGotchiViewModel.Components.First();
        return result.ImageUrl;
    }

    private IEnumerable<IGrouping<string, string>> GetComponentsByCategories(InnoGotchiComponentCollectionDto data)
    {
        var serverAddress = LocalStorageService.GetServerAddress();
        return data.Components.GroupBy(x => x.Name,
                s => serverAddress.Combine(s.ImageUrl).ToString());
    }

}