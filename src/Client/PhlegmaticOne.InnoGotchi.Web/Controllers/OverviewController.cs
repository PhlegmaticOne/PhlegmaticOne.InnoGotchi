﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Farms;
using PhlegmaticOne.InnoGotchi.Web.Requests.Overviews;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Farms;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.FarmStatistics;
using PhlegmaticOne.LocalStorage;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class OverviewController : ClientRequestsController
{
    public OverviewController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper)
    { }

    [HttpGet]
    public Task<IActionResult> My()
    {
        return FromAuthorizedGet(new GetIsFarmExistsRequest(), exists =>
        {
            IActionResult result = exists ?
                View() :
                View("~/Views/Farms/Create.cshtml", new CreateFarmViewModel
                {
                    ErrorMessage = AppErrorMessages.FarmDoesNotExistMessage
                });
            return Task.FromResult(result);
        });
    }

    [HttpGet]
    public Task<IActionResult> Collaborated()
    {
        return FromAuthorizedGet(new GetCollaboratedFarmStatisticsRequest(), statistics =>
        {
            var mapped = Mapper.Map<IList<PreviewFarmStatisticsViewModel>>(statistics);
            IActionResult view = View(mapped);
            return Task.FromResult(view);
        });
    }

    [HttpGet]
    public Task<IActionResult> MyPreviewPartial()
    {
        return FromAuthorizedGet(new GetPreviewFarmStatisticsRequest(), statistics =>
        {
            var viewModel = Mapper.Map<PreviewFarmStatisticsViewModel>(statistics);
            IActionResult view = PartialView("~/Views/_Partial_Views/Overview/PreviewFarmStatistics.cshtml", viewModel);
            return Task.FromResult(view);
        });
    }

    [HttpGet]
    public Task<IActionResult> MyDetailedPartial()
    {
        return FromAuthorizedGet(new GetDetailedFarmStatisticsRequest(), statistics =>
        {
            var viewModel = Mapper.Map<DetailedFarmStatisticsViewModel>(statistics);
            IActionResult view = PartialView("~/Views/_Partial_Views/Overview/DetailedFarmStatistics.cshtml", viewModel);
            return Task.FromResult(view);
        });
    }
}