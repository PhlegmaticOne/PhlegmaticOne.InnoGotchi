using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared.Dtos;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class AuthenticationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Authenticate(AuthenticationViewModel authenticationViewModel)
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
}