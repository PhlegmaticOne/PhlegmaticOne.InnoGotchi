using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory) => 
        _httpClientFactory = httpClientFactory;

    [HttpGet]
    public IActionResult Login() => View();

    [HttpGet]
    public IActionResult Register() => View();


    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        return View();
    }

    public async Task<IActionResult> Login(LoginViewModel authenticationViewModel)
    {
        var userCredentials = new UserCredentials
        {
            Email = authenticationViewModel.Email,
            Password = authenticationViewModel.Password
        };

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7142/api/");

        var httpResponseMessage = 
            await client.PostAsJsonAsync("JwtAuthentication/Authenticate", userCredentials);


        
        if (!httpResponseMessage.IsSuccessStatusCode) return NotFound();

        var result = await httpResponseMessage.Content.ReadFromJsonAsync<JwtTokenDto>();


        return Ok(result);
    }

    private async Task<JwtTokenDto?> GetJwtToken(string email, string password)
    {
        var userCredentials = new UserCredentials
        {
            Email = email,
            Password = password
        };

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7142/api/");

        var httpResponseMessage =
            await client.PostAsJsonAsync("JwtAuthentication/Authenticate", userCredentials);


        return httpResponseMessage.IsSuccessStatusCode ? 
            await httpResponseMessage.Content.ReadFromJsonAsync<JwtTokenDto>() :
            null;
    }

    private async Task AuthenticateByCookies()
    {

    }
}