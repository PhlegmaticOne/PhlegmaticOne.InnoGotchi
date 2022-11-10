﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Helpers;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;
using System.Security.Claims;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[AllowAnonymous]
public class AccountController : ClientRequestsController
{
    private readonly IMapper _mapper;

    public AccountController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IMapper mapper) : base(clientRequestsService, localStorageService) =>
        _mapper = mapper;


    [HttpGet]
    public IActionResult Login(string returnUrl) => View();

    [HttpGet]
    public IActionResult Register() => View();


    [HttpPost]
    public Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        var registerDto = _mapper.Map<RegisterProfileDto>(registerViewModel);

        return FromAuthorizedPost(new RegisterProfileRequest(registerDto), async profile =>
        {
            await AuthenticateAsync(profile);
            return LocalRedirect(Constants.HomeUrl);
        });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var loginDto = _mapper.Map<LoginDto>(loginViewModel);

        return await FromAuthorizedPost(new LoginRequest(loginDto), async profile =>
        {
            await AuthenticateAsync(profile);
            return LocalRedirect(loginViewModel.ReturnUrl ?? Constants.HomeUrl);
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