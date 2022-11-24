using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Collaborations;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class CollaborationsController : ClientRequestsController
{
    public CollaborationsController(IClientRequestsService clientRequestsService, 
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper) { }

    [HttpPost]
    public Task<IActionResult> Collaborate([FromBody] IdDto idDto)
    {
        return FromAuthorizedPost(new CreateCollaborationRequest(idDto), dto =>
        {
            IActionResult ok = Ok();
            return Task.FromResult(ok);
        });
    }
}