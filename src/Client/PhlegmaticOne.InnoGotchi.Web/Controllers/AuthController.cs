using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Profile;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;
using PhlegmaticOne.LocalStorage;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[AllowAnonymous]
public class AuthController : ClientRequestsController
{
    private readonly IValidator<LoginViewModel> _loginViewModelValidator;
    private readonly IValidator<RegisterViewModel> _registerViewModelValidator;

    public AuthController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IValidator<RegisterViewModel> registerViewModelValidator,
        IValidator<LoginViewModel> loginViewModelValidator,
        IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper)
    {
        _registerViewModelValidator = registerViewModelValidator;
        _loginViewModelValidator = loginViewModelValidator;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        var validationResult = await _registerViewModelValidator.ValidateAsync(registerViewModel);

        if (validationResult.IsValid == false)
            return ViewWithErrorsFromValidationResult(validationResult, nameof(Register), registerViewModel);

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
        var validationResult = await _loginViewModelValidator.ValidateAsync(loginViewModel);

        if (validationResult.IsValid == false)
            return ViewWithErrorsFromValidationResult(validationResult, nameof(Login), loginViewModel);

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