﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;
using PhlegmaticOne.LocalStorage;
using PhlegmaticOne.PagedLists.Implementation;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class InnoGotchiesController : ClientRequestsController
{
    public InnoGotchiesController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper)
    {
    }

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
    public Task<IActionResult> Pet(Guid petId)
    {
        return FromAuthorizedGet(new GetDetailedInnoGotchiRequest(petId), InnoGotchiView);
    }

    [HttpPost]
    public Task<IActionResult> Feed(InnoGotchiActionViewModel innoGotchiActionViewModel)
    {
        var model = FeedModel(innoGotchiActionViewModel.InnoGotchiId);
        return FromAuthorizedPut(new UpdateInnoGotchiRequest(model), _ =>
        {
            return FromAuthorizedGet(new GetDetailedInnoGotchiRequest(innoGotchiActionViewModel.InnoGotchiId),
                InnoGotchiView);
        }, result => ErrorView(result.ErrorMessage!));
    }

    [HttpPost]
    public Task<IActionResult> Drink(InnoGotchiActionViewModel innoGotchiActionViewModel)
    {
        var model = DrinkModel(innoGotchiActionViewModel.InnoGotchiId);
        return FromAuthorizedPut(new UpdateInnoGotchiRequest(model), result =>
        {
            return FromAuthorizedGet(new GetDetailedInnoGotchiRequest(innoGotchiActionViewModel.InnoGotchiId),
                InnoGotchiView);
        }, result => ErrorView(result.ErrorMessage!));
    }

    [HttpPost]
    public Task<IActionResult> FeedPartial([FromBody] InnoGotchiRequestViewModel request)
    {
        var model = FeedModel(request.Id);
        return FromAuthorizedPut(new UpdateInnoGotchiRequest(model), result =>
        {
            return FromAuthorizedGet(new GetPreviewInnoGotchiRequest(request.Id),
                dto => InnoGotchiCardPartialView(dto, request.CanSeeDetails));
        }, result => ErrorView(result.ErrorMessage!));
    }

    [HttpPost]
    public Task<IActionResult> DrinkPartial([FromBody] InnoGotchiRequestViewModel request)
    {
        var model = DrinkModel(request.Id);
        return FromAuthorizedPut(new UpdateInnoGotchiRequest(model), result =>
        {
            return FromAuthorizedGet(new GetPreviewInnoGotchiRequest(request.Id),
                dto => InnoGotchiCardPartialView(dto, request.CanSeeDetails));
        }, result => ErrorView(result.ErrorMessage!));
    }

    private Task<IActionResult> InnoGotchiView(DetailedInnoGotchiDto innoGotchi)
    {
        var result = Mapper.Map<DetailedInnoGotchiViewModel>(innoGotchi);
        IActionResult view = View(nameof(Pet), result);
        return Task.FromResult(view);
    }

    private Task<IActionResult> InnoGotchiCardPartialView(PreviewInnoGotchiDto innoGotchi, bool canSeeDetails)
    {
        ViewData["CanSeeDetails"] = canSeeDetails;
        var result = Mapper.Map<PreviewInnoGotchiViewModel>(innoGotchi);
        IActionResult view = PartialView("~/Views/_Partial_Views/InnoGotchies/InnoGotchiCard.cshtml", result);
        return Task.FromResult(view);
    }

    private static UpdateInnoGotchiDto FeedModel(Guid petId)
    {
        return new()
        {
            InnoGotchiOperationType = InnoGotchiOperationType.Feeding,
            PetId = petId
        };
    }

    private static UpdateInnoGotchiDto DrinkModel(Guid petId)
    {
        return new()
        {
            InnoGotchiOperationType = InnoGotchiOperationType.Drinking,
            PetId = petId
        };
    }
}