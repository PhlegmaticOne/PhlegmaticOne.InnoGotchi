using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class InvitationsController : ClientRequestsController
{
    public InvitationsController(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService) :
        base(clientRequestsService, localStorageService) { }

    [HttpGet]
    public IActionResult Index() => View();
}