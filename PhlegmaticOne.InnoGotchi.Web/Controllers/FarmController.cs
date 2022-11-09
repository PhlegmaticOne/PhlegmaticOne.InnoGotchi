using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Farms;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Farms;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class FarmController : ClientRequestsController
{
    private readonly IMapper _mapper;

    public FarmController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService) =>
        _mapper = mapper;

    [HttpGet]
    public IActionResult Create() => View();

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var email = User.Identity!.Name!;
        var serverResponse =
            await SendAuthorizedGetRequestAsync<FarmDto>(new GetFarmRequest(email));

        if (serverResponse.IsSuccess == false)
        {
            return BadRequest();
        }

        var operationResult = serverResponse.ResponseData!;

        if (operationResult.IsSuccess == false)
        {
            return RedirectToAction(nameof(Create));
        }

        var farmDto = serverResponse.GetData<FarmDto>();
        var farmViewModel = _mapper.Map<FarmViewModel>(farmDto);

        return View(farmViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFarmViewModel createFarmViewModel)
    {
        var createFarmDto = _mapper.Map<CreateFarmDto>(createFarmViewModel);
        var serverResponse =
            await SendAuthorizedPostRequestAsync<FarmDto>(new CreateFarmRequest(createFarmDto));

        if (serverResponse.IsSuccess == false)
        {
            return BadRequest();
        }

        var data = serverResponse.GetData<FarmDto>();
        var viewModel = _mapper.Map<FarmViewModel>(data);

        return View(nameof(Index), viewModel);
    }
}