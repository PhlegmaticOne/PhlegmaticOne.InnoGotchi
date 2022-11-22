using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.FarmStatistics;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class FarmsController : ClientRequestsController
{
    private readonly IMapper _mapper;

    public FarmsController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult My() => View();

    [HttpGet]
    public Task<IActionResult> MyPreviewPartial()
    {
        return FromAuthorizedGet(new GetPreviewFarmStatisticsGetRequest(), statistics =>
        {
            var viewModel = _mapper.Map<PreviewFarmStatisticsViewModel>(statistics);
            IActionResult view = PartialView("PreviewFarmStatisticsPartialView", viewModel);
            return Task.FromResult(view);
        });
    }

    [HttpGet]
    public Task<IActionResult> MyDetailedPartial()
    {
        return FromAuthorizedGet(new GetDetailedFarmStatisticsGetRequest(), statistics =>
        {
            var viewModel = _mapper.Map<DetailedFarmStatisticsViewModel>(statistics);
            IActionResult view = PartialView("DetailedFarmStatisticsPartialView", viewModel);
            return Task.FromResult(view);
        });
    }
}