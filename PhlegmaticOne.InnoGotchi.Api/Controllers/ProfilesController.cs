using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ProfilesController : IdentityController
{
    private readonly IProfileAuthorizedActionsManager _profileAuthorizedActionsManager;
    private readonly ISearchProfilesManager _searchProfilesManager;

    public ProfilesController(IProfileAuthorizedActionsManager profileAuthorizedActionsManager, 
        ISearchProfilesManager searchProfilesManager)
    {
        _profileAuthorizedActionsManager = profileAuthorizedActionsManager;
        _searchProfilesManager = searchProfilesManager;
    }

    [HttpGet]
    public Task<OperationResult<DetailedProfileDto>> GetAuthorized() =>
        _profileAuthorizedActionsManager.GetDetailedAsync(ProfileId());

    [HttpGet]
    public Task<OperationResult<IList<SearchProfileDto>>> Search(string searchText) =>
        _searchProfilesManager.SearchAsync(IdentityModel(searchText));

    [HttpPut]
    public Task<OperationResult<AuthorizedProfileDto>> Update([FromBody] UpdateProfileDto updateProfileDto) => 
        _profileAuthorizedActionsManager.UpdateAsync(IdentityModel(updateProfileDto));
}