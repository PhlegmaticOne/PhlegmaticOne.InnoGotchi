using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Helpers;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[AllowAnonymous]
public class AccountController : ClientRequestsController
{
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterViewModel> _registerViewModelValidator;
    private readonly IValidator<LoginViewModel> _loginViewModelValidator;

    public AccountController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IMapper mapper,
        IValidator<RegisterViewModel> registerViewModelValidator,
        IValidator<LoginViewModel> loginViewModelValidator) : 
        base(clientRequestsService, localStorageService)
    {
        _mapper = mapper;
        _registerViewModelValidator = registerViewModelValidator;
        _loginViewModelValidator = loginViewModelValidator;
    }


    [HttpGet]
    public IActionResult Login(string returnUrl) => View();

    [HttpGet]
    public IActionResult Register() => View();

    [HttpGet]
    [Authorize]
    public Task<IActionResult> Details()
    {
        return FromAuthorizedGet(new DetailedProfileGetRequest(), profile =>
        {
            var profileViewModel = _mapper.Map<ProfileViewModel>(profile);
            IActionResult view = View(nameof(Details), profileViewModel);
            return Task.FromResult(view);
        });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        var validationResult = await _registerViewModelValidator.ValidateAsync(registerViewModel);

        if (validationResult.IsValid == false)
        {
            return AddErrorsAndReturnView(validationResult, nameof(Register), registerViewModel);
        }

        var registerDto = _mapper.Map<RegisterProfileDto>(registerViewModel);

        return await FromAuthorizedPost(new RegisterProfileRequest(registerDto), async profile =>
        {
            await AuthenticateAsync(profile);
            return RedirectToAction(nameof(Details));
        });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var validationResult = await _loginViewModelValidator.ValidateAsync(loginViewModel);

        if (validationResult.IsValid == false)
        {
            return AddErrorsAndReturnView(validationResult, nameof(Login), loginViewModel);
        }

        var loginDto = _mapper.Map<LoginDto>(loginViewModel);

        return await FromAuthorizedPost(new LoginRequest(loginDto), async profile =>
        {
            await AuthenticateAsync(profile);
            return RedirectToAction(nameof(Details));
        });
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateAccountViewModel updateAccountViewModel)
    {
        return RedirectToAction(nameof(Details));
    }

    private async Task AuthenticateAsync(AuthorizedProfileDto profileDto)
    {
        var claimsPrincipal = ClaimsPrincipalGenerator.GenerateClaimsPrincipal(profileDto);
        await SignInAsync(claimsPrincipal, profileDto.JwtToken.Token!);
    }
}