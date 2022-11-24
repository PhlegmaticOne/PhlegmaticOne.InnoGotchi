using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.PagedLists;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class InnoGotchiesController : ClientRequestsController
{
    public InnoGotchiesController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper) { }

    [HttpGet]
    public Task<IActionResult> All(int? pageIndex, int? pageSize, int? sortType, bool? isAscending)
    {
        var pagedListData = new PagedListData
        {
            PageIndex = pageIndex is null ? 0 : pageIndex.Value - 1,
            PageSize = pageSize ?? 15,
            SortType = sortType ?? 0,
            IsAscending = isAscending ?? false
        };

        return FromAuthorizedGet(new GetInnoGotchiesPagedListRequest(pagedListData), list =>
        {
            ViewData["PageSize"] = pagedListData.PageSize;
            ViewData["SortType"] = pagedListData.SortType;
            ViewData["IsAscending"] = pagedListData.IsAscending;

            var mapped = Mapper.Map<PagedList<ReadonlyInnoGotchiPreviewViewModel>>(list);
            IActionResult view = View(mapped);
            return Task.FromResult(view);
        });
    }

    [HttpGet]
    public Task<IActionResult> Pet(Guid petId) => 
        FromAuthorizedGet(new GetInnoGotchiRequest(petId), InnoGotchiView);

    [HttpPost]
    public Task<IActionResult> Feed(InnoGotchiActionViewModel innoGotchiActionViewModel)
    {
        var identityDto = Mapper.Map<IdDto>(innoGotchiActionViewModel);
        return FromAuthorizedPut(new FeedInnoGotchiRequest(identityDto), InnoGotchiView);
    }

    [HttpPost]
    public Task<IActionResult> Drink(InnoGotchiActionViewModel innoGotchiActionViewModel)
    {
        var identityDto = Mapper.Map<IdDto>(innoGotchiActionViewModel);
        return FromAuthorizedPut(new DrinkInnoGotchiRequest(identityDto), InnoGotchiView);
    }

    [HttpPost]
    public Task<IActionResult> FeedPartial([FromBody] InnoGotchiRequestViewModel request) => 
        FromAuthorizedPut(new FeedInnoGotchiRequest(new IdDto(request.Id)),
            result => InnoGotchiCardPartialView(result, request.CanSeeDetails));

    [HttpPost]
    public Task<IActionResult> DrinkPartial([FromBody] InnoGotchiRequestViewModel request) => 
        FromAuthorizedPut(new DrinkInnoGotchiRequest(new IdDto(request.Id)),
            result => InnoGotchiCardPartialView(result, request.CanSeeDetails));

    private Task<IActionResult> InnoGotchiView(DetailedInnoGotchiDto innoGotchi)
    {
        var result = Mapper.Map<DetailedInnoGotchiViewModel>(innoGotchi);
        IActionResult view = View(nameof(Pet), result);
        return Task.FromResult(view);
    }

    private Task<IActionResult> InnoGotchiCardPartialView(DetailedInnoGotchiDto innoGotchi, bool canSeeDetails)
    {
        ViewData["CanSeeDetails"] = canSeeDetails;
        var result = Mapper.Map<PreviewInnoGotchiViewModel>(innoGotchi);
        IActionResult view = PartialView("~/Views/_Partial_Views/InnoGotchies/InnoGotchiCard.cshtml", result);
        return Task.FromResult(view);
    }
}