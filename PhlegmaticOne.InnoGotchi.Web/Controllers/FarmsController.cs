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
public class FarmsController : ClientRequestsController
{
    private readonly IMapper _mapper;
    public FarmsController(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService,
        IMapper mapper) : base(clientRequestsService, localStorageService) =>
        _mapper = mapper;


    [HttpGet]
    public IActionResult Create() => View();

    [HttpGet]
    public Task<IActionResult> My()
    {
        return FromAuthorizedGet(new GetFarmRequest(), farm =>
        {
            var farmViewModel = _mapper.Map<DetailedFarmViewModel>(farm);
            IActionResult view = View(farmViewModel);
            return Task.FromResult(view);
        }, onOperationFailed: _ => RedirectToAction(nameof(Create)));
    }

    [HttpGet]
    public Task<IActionResult> Collaborated()
    {
        return FromAuthorizedGet(new GetCollaboratorsFarmPreviewsGetRequest(), dtos =>
        {
            var viewModels = _mapper.Map<IList<PreviewFarmViewModel>>(dtos);
            IActionResult view = View(viewModels);
            return Task.FromResult(view);
        });
    }

    [HttpGet]
    public Task<IActionResult> View(Guid profileId)
    {
        return FromAuthorizedGet(new GetProfileFarmGetRequest(profileId), dto =>
        {
            var mapped = _mapper.Map<DetailedFarmViewModel>(dto);
            IActionResult view = View(mapped);
            return Task.FromResult(view);
        });
    }

    [HttpPost]
    public Task<IActionResult> Create(CreateFarmViewModel createFarmViewModel)
    {
        var createFarmDto = _mapper.Map<CreateFarmDto>(createFarmViewModel);

        return FromAuthorizedPost(new CreateFarmRequest(createFarmDto), _ =>
        {
            IActionResult view = RedirectToAction(nameof(My));
            return Task.FromResult(view);
        }, result => ViewWithErrorsFromOperationResult(result, nameof(Create), createFarmViewModel));
    }
}