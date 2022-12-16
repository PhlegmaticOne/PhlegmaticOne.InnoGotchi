using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Overviews;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Other;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Statistics;
using PhlegmaticOne.LocalStorage;

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
        return FromAuthorizedGet(new GetGlobalStatisticsRequest(), dto =>
        {
            ViewData["BuildCountStatistics"] = dto.PetsTotalCount != 0;
            ViewData["BuildAgeStatistics"] = dto.DeadPetsAverageAge != 0 || dto.AlivePetsAverageAge != 0;
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