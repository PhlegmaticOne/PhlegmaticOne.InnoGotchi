﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Services.Storage;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class AccountController : Controller
{
    private readonly IClientRequestsService _clientRequestsService;
    private readonly ILocalStorageService _localStorageService;
    private readonly IMapper _mapper;

    public AccountController(IClientRequestsService clientRequestsService, 
        ILocalStorageService localStorageService,
        IMapper mapper)
    {
        _clientRequestsService = clientRequestsService;
        _localStorageService = localStorageService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpGet]
    public IActionResult Register() => View();


    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        var registerDto = _mapper.Map<RegisterProfileDto>(registerViewModel);

        var serverResponse = await _clientRequestsService
            .PostAsync<ProfileDto>(new RegisterProfileRequest(registerDto));

        if (serverResponse.IsSuccess == false)
        {
            return BadRequest();
        }

        _localStorageService.SetIsAuthenticationRequired(false);

        var data = serverResponse.GetData<ProfileDto>();
        SetJwtToken(data);

        return LocalRedirect("~/");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var loginDto = _mapper.Map<LoginDto>(loginViewModel);

        var result = await _clientRequestsService
            .PostAsync<ProfileDto>(new LoginRequest(loginDto));

        _localStorageService.SetIsAuthenticationRequired(false);

        return Ok(result);
    }

    private void SetJwtToken(ProfileDto profile) => _localStorageService.SetJwtToken(profile.JwtToken.Token!);
}