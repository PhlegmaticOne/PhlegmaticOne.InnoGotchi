﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Overviews;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Other;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Statistics;
using PhlegmaticOne.LocalStorage;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class HomeController : ClientRequestsController
{
    public HomeController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper) { }

    [HttpGet]
    public Task<IActionResult> Index()
    {
        if (User.Identity!.IsAuthenticated == false)
        {
            ViewData["BuildStatistics"] = false;
            IActionResult view = View(model: new GlobalStatisticsViewModel());
            return Task.FromResult(view);
        }

        return FromAuthorizedGet(new GetGlobalStatisticsRequest(), dto =>
        {
            ViewData["BuildStatistics"] = true;
            var viewModel = Mapper.Map<GlobalStatisticsViewModel>(dto);
            IActionResult view = View(viewModel);
            return Task.FromResult(view);
        });
    }

    public IActionResult Error(string errorMessage) =>
        View(new ErrorViewModel
        {
            ErrorMessage = errorMessage
        });
}