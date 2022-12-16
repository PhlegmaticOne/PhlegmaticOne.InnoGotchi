using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Profile;
using PhlegmaticOne.LocalStorage;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class AvatarController : ClientRequestsController
{
    public AvatarController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService,
        IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper) { }

    [HttpGet]
    public Task<IActionResult> Get()
    {
        return FromAuthorizedGet(new GetAvatarRequest(), avatarData =>
        {
            IActionResult result = File(avatarData, "image/png", "image.png");
            return Task.FromResult(result);
        });
    }
}