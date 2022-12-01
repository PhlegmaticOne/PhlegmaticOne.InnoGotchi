using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Profiles;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Accounts;
using PhlegmaticOne.LocalStorage;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class AccountsController : ClientRequestsController
{
    public AccountsController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService, mapper)
    { }

    [HttpGet]
    public Task<IActionResult> SearchPartial(string searchText)
    {
        return FromAuthorizedGet(new SearchProfilesRequest(searchText), profiles =>
        {
            var viewModels = Mapper.Map<IList<SearchProfileViewModel>>(profiles);
            IActionResult view = PartialView("~/Views/_Partial_Views/Profiles/ProfileSearchResultsList.cshtml", viewModels);
            return Task.FromResult(view);
        });
    }
}