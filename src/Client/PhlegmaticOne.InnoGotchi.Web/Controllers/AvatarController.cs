using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Profile;
using PhlegmaticOne.LocalStorage;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class AvatarController : ClientRequestsController
{
    public AvatarController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await ClientRequestsService.GetAsync(new GetAvatarRequest(), JwtToken());
        var data = result.GetData()!;
        return File(data, "image/png", "image.png");
    }
}