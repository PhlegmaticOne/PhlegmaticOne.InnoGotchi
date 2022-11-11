using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Constructor;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class ConstructorController : ClientRequestsController
{
    private readonly IMapper _mapper;

    public ConstructorController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IMapper mapper) :
        base(clientRequestsService, localStorageService)
    {
        _mapper = mapper;
    }

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
    public Task<IActionResult> CreateNew([FromBody] CreateInnoGotchiViewModel createInnoGotchiViewModel)
    {
        var createInnoGotchiDto = _mapper.Map<CreateInnoGotchiDto>(createInnoGotchiViewModel);

        return FromAuthorizedPost(new CreateInnoGotchiRequest(createInnoGotchiDto), innoGotchi =>
        {
            IActionResult result = View("/Farm/Index");
            return Task.FromResult(result);
        });
    }

    private IEnumerable<IGrouping<string, string>> GetComponentsByCategories(InnoGotchiComponentCollectionDto data)
    {
        var serverAddress = LocalStorageService.GetServerAddress()!;
        return data.Components.GroupBy(x => x.Name,
                s => serverAddress.Combine(s.ImageUrl).ToString());
    }
}