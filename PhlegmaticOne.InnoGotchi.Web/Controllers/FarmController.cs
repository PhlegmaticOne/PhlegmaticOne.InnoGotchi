using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Farms;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class FarmController : ClientRequestsController
{
    private readonly IMapper _mapper;
    public FarmController(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService,
        IMapper mapper) : base(clientRequestsService, localStorageService) =>
        _mapper = mapper;


    [HttpGet]
    public IActionResult Create() => View();

    [HttpGet]
    public Task<IActionResult> Details()
    {
        return FromAuthorizedGet(new GetFarmRequest(), farm =>
        {
            var farmViewModel = _mapper.Map<DetailedFarmViewModel>(farm);
            IActionResult view = View(farmViewModel);
            return Task.FromResult(view);
        }, onOperationFailed: _ => RedirectToAction(nameof(Create)));
    }

    [HttpPost]
    public Task<IActionResult> Create(CreateFarmViewModel createFarmViewModel)
    {
        var createFarmDto = _mapper.Map<CreateFarmDto>(createFarmViewModel);

        return FromAuthorizedPost(new CreateFarmRequest(createFarmDto), _ =>
        {
            IActionResult view = RedirectToAction(nameof(Details));
            return Task.FromResult(view);
        }, result => ViewWithErrorsFromOperationResult(result, nameof(Create), createFarmViewModel));
    }
}