using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Constructor;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;
using PhlegmaticOne.LocalStorage;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class ConstructorController : ClientRequestsController
{
    public ConstructorController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper) { }

    [HttpGet]
    public Task<IActionResult> Create()
    {
        return FromAuthorizedGet(new GetAllInnoGotchiComponentsRequest(), componentsDto =>
        {
            var mapped = Mapper.Map<ConstructorViewModel>(componentsDto);
            IActionResult view = View(mapped);
            return Task.FromResult(view);
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInnoGotchiViewModel createInnoGotchiViewModel)
    {
        var createInnoGotchiDto = Mapper.Map<CreateInnoGotchiDto>(createInnoGotchiViewModel);

        return await FromAuthorizedPost(new CreateInnoGotchiRequest(createInnoGotchiDto), _ =>
        {
            var result = CreateInnoGotchiPartialView(createInnoGotchiViewModel);
            return Task.FromResult(result);
        }, result =>
        {
            createInnoGotchiViewModel.ErrorMessage = result.ErrorMessage;
            return CreateInnoGotchiPartialView(createInnoGotchiViewModel);
        });
    }

    [HttpPost]
    public IActionResult CategoryList([FromBody] ComponentCategoryViewModel categoryViewModel, [FromQuery] int orderInLayer)
    {
        ViewData["OrderInLayer"] = orderInLayer;
        return PartialView("~/Views/_Partial_Views/Constructor/CategoryImagesList.cshtml", categoryViewModel);
    }

    private IActionResult CreateInnoGotchiPartialView(CreateInnoGotchiViewModel createInnoGotchiViewModel) => 
        PartialView("~/Views/_Partial_Views/Constructor/CreateInnoGotchiArea.cshtml", createInnoGotchiViewModel);
}