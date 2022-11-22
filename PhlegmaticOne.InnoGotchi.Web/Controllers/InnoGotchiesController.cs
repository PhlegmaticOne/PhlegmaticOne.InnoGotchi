using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.PagedLists;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class InnoGotchiesController : ClientRequestsController
{
    private readonly IMapper _mapper;

    public InnoGotchiesController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IMapper mapper) :
        base(clientRequestsService, localStorageService) => _mapper = mapper;

    [HttpGet]
    public Task<IActionResult> All(int? pageIndex)
    {
        return FromAuthorizedGet(new GetPagedListRequest(pageIndex is null ? 0 : pageIndex.Value - 1), list =>
        {
            var mapped = _mapper.Map<PagedList<PreviewInnoGotchiViewModel>>(list);
            IActionResult view = View(mapped);
            return Task.FromResult(view);
        });
    }

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

    [HttpPost]
    public Task<IActionResult> FeedPartial([FromBody] IdDto innoGotchiIdDto)
    {
        return FromAuthorizedPut(new FeedInnoGotchiRequest(innoGotchiIdDto), InnoGotchiCardPartialView);
    }

    [HttpPost]
    public Task<IActionResult> DrinkPartial([FromBody] IdDto innoGotchiIdDto)
    {
        return FromAuthorizedPut(new DrinkInnoGotchiRequest(innoGotchiIdDto), InnoGotchiCardPartialView);
    }

    private Task<IActionResult> InnoGotchiView(DetailedInnoGotchiDto innoGotchi)
    {
        var result = _mapper.Map<DetailedInnoGotchiViewModel>(innoGotchi);
        IActionResult view = View(nameof(Pet), result);
        return Task.FromResult(view);
    }

    private Task<IActionResult> InnoGotchiCardPartialView(DetailedInnoGotchiDto innoGotchi)
    {
        var result = _mapper.Map<PreviewInnoGotchiViewModel>(innoGotchi);
        IActionResult view = PartialView("MyInnoGotchiCardPartialView", result);
        return Task.FromResult(view);
    }
}