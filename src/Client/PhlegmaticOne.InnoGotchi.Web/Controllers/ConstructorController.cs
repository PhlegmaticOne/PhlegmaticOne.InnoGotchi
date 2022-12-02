using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Constructor;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;
using PhlegmaticOne.LocalStorage;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class ConstructorController : ClientRequestsController
{
    private readonly IValidator<CreateInnoGotchiViewModel> _createInnoGotchiViewModelValidator;

    public ConstructorController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper,
        IValidator<CreateInnoGotchiViewModel> createInnoGotchiViewModelValidator) :
        base(clientRequestsService, localStorageService, mapper)
    {
        _createInnoGotchiViewModelValidator = createInnoGotchiViewModelValidator;
    }

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
        var validationResult = await _createInnoGotchiViewModelValidator.ValidateAsync(createInnoGotchiViewModel);

        if (validationResult.IsValid == false)
        {
            validationResult.AddToModelState(ModelState);
            createInnoGotchiViewModel.ErrorMessage = validationResult.ToString(".");
            return CreateInnoGotchiPartialView(createInnoGotchiViewModel);
        }

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

    private IActionResult CreateInnoGotchiPartialView(CreateInnoGotchiViewModel createInnoGotchiViewModel)
    {
        return PartialView("~/Views/_Partial_Views/Constructor/CreateInnoGotchiArea.cshtml", createInnoGotchiViewModel);
    }
}