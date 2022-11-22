using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ProfilesController : IdentityController
{
    private readonly IProfileAuthorizedActionsManager _profileAuthorizedActionsManager;
    private readonly ISearchProfilesManager _searchProfilesManager;
    private readonly IDefaultAvatarService _avatarConvertingService;

    public ProfilesController(IProfileAuthorizedActionsManager profileAuthorizedActionsManager, 
        ISearchProfilesManager searchProfilesManager,
        IDefaultAvatarService avatarConvertingService)
    {
        _profileAuthorizedActionsManager = profileAuthorizedActionsManager;
        _searchProfilesManager = searchProfilesManager;
        _avatarConvertingService = avatarConvertingService;
    }

    [HttpPost]
    public Task<OperationResult<AuthorizedProfileDto>> Update([FromBody] UpdateProfileDto updateProfileDto) => 
        _profileAuthorizedActionsManager.UpdateAsync(updateProfileDto);

    [HttpGet]
    public async Task<OperationResult<DetailedProfileDto>> GetDetailed()
    {
        var profileOperationResult = await _profileAuthorizedActionsManager.GetDetailedAsync(ProfileId());

        var profile = profileOperationResult.Result!;
        if (profile.AvatarData.Any() == false)
        {
            profile.AvatarData = await _avatarConvertingService.GetDefaultAvatarDataAsync();
        }

        return profileOperationResult;
    }

    [HttpGet]
    public Task<OperationResult<IList<SearchProfileDto>>> Search(string searchText) => 
        _searchProfilesManager.SearchAsync(searchText);
}