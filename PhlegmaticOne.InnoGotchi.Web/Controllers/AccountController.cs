using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.InnoGotchi.Shared.OperationResults;
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

        var result =
            await _clientRequestsService
                .PostAsync<OperationResult<ProfileDto>>(new RegisterProfileRequest(registerDto));

        if (result.IsSuccess == false)
        {
            return BadRequest();
        }

        var responseResult = result.ResponseData!.Result;
        SetJwtToken(responseResult!);

        return LocalRedirect("~/Home/Index");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var loginDto = _mapper.Map<LoginDto>(loginViewModel);

        var result =
            await _clientRequestsService
                .PostAsync<OperationResult<ProfileDto>>(new LoginRequest(loginDto));

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        var jwtToken = _localStorageService.GetJwtToken();

        var result = await _clientRequestsService
            .GetAsync<OperationResult<string>>(new TestGetRequest(22), jwtToken);

        if (result.IsSuccess == false)
        {
            ViewBag.Result = result.ReasonPhrase;
            return View();
        }

        ViewBag.Result = result.ResponseData.Result;
        return View();
    }

    private void SetJwtToken(ProfileDto profile) => _localStorageService.SetJwtToken(profile.JwtToken.Token!);
}