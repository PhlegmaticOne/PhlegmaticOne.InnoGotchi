using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class InnoGotchiesController : ClientRequestsController
{
    private readonly IMapper _mapper;

    public InnoGotchiesController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IMapper mapper) :
        base(clientRequestsService, localStorageService) => _mapper = mapper;

    [HttpGet]
    public Task<IActionResult> Pet(Guid petId) => 
        FromAuthorizedGet(new GetInnoGotchiGetRequest(petId), InnoGotchiView);

    [HttpPost]
    public Task<IActionResult> Feed(InnoGotchiActionViewModel innoGotchiActionViewModel)
    {
        var identityDto = _mapper.Map<IdDto>(innoGotchiActionViewModel);
        return FromAuthorizedPut(new FeedInnoGotchiRequest(identityDto), InnoGotchiView);
    }

    [HttpPost]
    public Task<IActionResult> Drink(InnoGotchiActionViewModel innoGotchiActionViewModel)
    {
        var identityDto = _mapper.Map<IdDto>(innoGotchiActionViewModel);
        return FromAuthorizedPut(new DrinkInnoGotchiRequest(identityDto), InnoGotchiView);
    }

    private Task<IActionResult> InnoGotchiView(DetailedInnoGotchiDto innoGotchi)
    {
        var result = _mapper.Map<DetailedInnoGotchiViewModel>(innoGotchi);
        IActionResult view = View(nameof(Pet), result);
        return Task.FromResult(view);
    }
}