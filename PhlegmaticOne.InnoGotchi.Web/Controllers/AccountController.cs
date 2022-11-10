using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;
using System.Security.Claims;
using FluentValidation;
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
            return ToHomeView();
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
            return loginViewModel.ReturnUrl is null ? ToHomeView() : LocalRedirect(loginViewModel.ReturnUrl);
        });
    }

    private async Task AuthenticateAsync(ProfileDto profileDto)
    {
        var claimsPrincipal = CreatePrincipalFromProfile(profileDto);
        await SignInAsync(claimsPrincipal, profileDto.JwtToken.Token!);
    }

    private static ClaimsPrincipal CreatePrincipalFromProfile(ProfileDto profileDto)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, profileDto.Email),
            new(ProfileClaimsConstants.FirstNameClaimName, profileDto.FirstName),
            new(ProfileClaimsConstants.SecondNameClaimName, profileDto.SecondName)
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            Constants.CookieAuthenticationType,
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return new(claimsIdentity);
    }
}