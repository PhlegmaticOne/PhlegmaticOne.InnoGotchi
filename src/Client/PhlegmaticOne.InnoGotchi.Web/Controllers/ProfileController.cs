using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Web.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Profile;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;
using PhlegmaticOne.LocalStorage;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

[Authorize]
public class ProfileController : ClientRequestsController
{
    private readonly IValidator<UpdateAccountViewModel> _updateAccountViewModel;

    public ProfileController(IClientRequestsService clientRequestsService,
        ILocalStorageService localStorageService, IMapper mapper,
        IValidator<UpdateAccountViewModel> updateAccountViewModel) :
        base(clientRequestsService, localStorageService, mapper)
    {
        _updateAccountViewModel = updateAccountViewModel;
    }

    [HttpGet]
    public Task<IActionResult> Details()
    {
        return FromAuthorizedGet(new GetDetailedProfileRequest(), profile =>
        {
            var profileViewModel = Mapper.Map<ProfileViewModel>(profile);
            IActionResult view = View(nameof(Details), profileViewModel);
            return Task.FromResult(view);
        });
    }


    [HttpGet]
    public Task<IActionResult> Update()
    {
        return FromAuthorizedGet(new GetDetailedProfileRequest(), profile =>
        {
            var profileViewModel = Mapper.Map<UpdateAccountViewModel>(profile);
            IActionResult view = View(nameof(Update), profileViewModel);
            return Task.FromResult(view);
        });
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateAccountViewModel updateAccountViewModel)
    {
        var validationResult = await _updateAccountViewModel.ValidateAsync(updateAccountViewModel);

        if (validationResult.IsValid == false)
            return ViewWithErrorsFromValidationResult(validationResult, nameof(Update), updateAccountViewModel);

        var updateDto = Mapper.Map<UpdateProfileDto>(updateAccountViewModel);

        return await FromAuthorizedPut(new UpdateProfileRequest(updateDto), async profile =>
        {
            await SignOutAsync();
            await AuthenticateAsync(profile);
            return RedirectToAction(nameof(Details));
        }, result => ViewWithErrorsFromOperationResult(result, nameof(Update), updateAccountViewModel));
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await SignOutAsync();
        return HomeView();
    }
}