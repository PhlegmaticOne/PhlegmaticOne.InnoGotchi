using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Collaborations;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.LocalStorage;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class CollaborationsController : ClientRequestsController
{
    public CollaborationsController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper) { }

    [HttpPost]
    public Task<IActionResult> Collaborate([FromBody] IdViewModel idViewModel)
    {
        var createCollaborationDto = new CreateCollaborationDto(idViewModel.Id);
        return FromAuthorizedPost(new CreateCollaborationRequest(createCollaborationDto), _ =>
        {
            IActionResult ok = Ok();
            return Task.FromResult(ok);
        });
    }
}