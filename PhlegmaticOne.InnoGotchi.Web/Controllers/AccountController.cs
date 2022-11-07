using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.InnoGotchi.Shared.OperationResults;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class AccountController : Controller
{
    private readonly IClientRequestsService _clientRequestsService;
    private readonly IMapper _mapper;

    public AccountController(IClientRequestsService clientRequestsService, IMapper mapper)
    {
        _clientRequestsService = clientRequestsService;
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

        return Ok(result);
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
        var result = await _clientRequestsService
            .GetAsync<OperationResult<string>>(new TestGetRequest(22));
        return View(result.ResponseData.Message);
    }
}