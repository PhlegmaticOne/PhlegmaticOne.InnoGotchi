using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Profile;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;
using PhlegmaticOne.LocalStorage;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[AllowAnonymous]
public class AuthController : ClientRequestsController
{
    private readonly IValidator<RegisterViewModel> _registerViewModelValidator;

    public AuthController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IValidator<RegisterViewModel> registerViewModelValidator,
        IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper)
    {
        _registerViewModelValidator = registerViewModelValidator;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl) => View();

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        var validationResult = await _registerViewModelValidator.ValidateAsync(registerViewModel);

        if (validationResult.IsValid == false)
        {
            return ViewWithErrorsFromValidationResult(validationResult, nameof(Register), registerViewModel);
        }

        var registerDto = Mapper.Map<RegisterProfileDto>(registerViewModel);

        return await FromAuthorizedPost(new RegisterProfileRequest(registerDto), async profile =>
        {
            await AuthenticateAsync(profile);
            return RedirectToAction("Details", "Profile");
        }, result => ViewWithErrorsFromOperationResult(result, nameof(Register), registerViewModel));
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var loginDto = Mapper.Map<LoginDto>(loginViewModel);

        return await FromAuthorizedPost(new LoginProfileRequest(loginDto), async profile =>
        {
            await AuthenticateAsync(profile);
            return loginViewModel.ReturnUrl is null
                ? RedirectToAction("Details", "Profile")
                : LocalRedirect(loginViewModel.ReturnUrl);
        }, result => ViewWithErrorsFromOperationResult(result, nameof(Login), loginViewModel));
    }
}