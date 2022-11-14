using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class ConstructorController : ClientRequestsController
{
    private readonly IMapper _mapper;
    private readonly IValidator<CreateInnoGotchiViewModel> _createInnoGotchiViewModelValidator;

    public ConstructorController(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService,
        IMapper mapper, IValidator<CreateInnoGotchiViewModel> createInnoGotchiViewModelValidator) : base(clientRequestsService, localStorageService)
    {
        _mapper = mapper;
        _createInnoGotchiViewModelValidator = createInnoGotchiViewModelValidator;
    }

    [HttpGet]
    public Task<IActionResult> Create()
    {
        return FromAuthorizedGet(new GetAllInnoGotchiComponentsRequest(), componentsDto =>
        {
            var mapped = _mapper.Map<ConstructorViewModel>(componentsDto);
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

        var createInnoGotchiDto = _mapper.Map<CreateInnoGotchiDto>(createInnoGotchiViewModel);

        return await FromAuthorizedPost(new CreateInnoGotchiRequest(createInnoGotchiDto), _ =>
        {
            IActionResult result = CreateInnoGotchiPartialView(createInnoGotchiViewModel);
            return Task.FromResult(result);
        }, result =>
        {
            result.AddErrorsToModelState(ModelState);
            createInnoGotchiViewModel.ErrorMessage = result.ErrorMessage;
            return CreateInnoGotchiPartialView(createInnoGotchiViewModel);
        });
    }

    private IActionResult CreateInnoGotchiPartialView(CreateInnoGotchiViewModel createInnoGotchiViewModel) => 
        PartialView("CreateInnoGotchiPartialView", createInnoGotchiViewModel);
}