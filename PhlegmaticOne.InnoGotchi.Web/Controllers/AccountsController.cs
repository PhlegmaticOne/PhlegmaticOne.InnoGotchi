using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.LocalStorage.Base;
using PhlegmaticOne.ServerRequesting.Services;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Accounts;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class AccountsController : ClientRequestsController
{
    private readonly IMapper _mapper;

    public AccountsController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper) :
        base(clientRequestsService, localStorageService)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public Task<IActionResult> SearchPartial(string searchText)
    {
        return FromAuthorizedGet(new SearchProfilesRequest(searchText), profiles =>
        {
            var viewModels = _mapper.Map<IList<SearchProfileViewModel>>(profiles);
            IActionResult view = PartialView("ProfilesSearchResultPartialView", viewModels);
            return Task.FromResult(view);
        });
    }
}