using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class SearchProfilesManager : ISearchProfilesManager
{
    private readonly ISearchProfilesService _searchProfilesService;
    private readonly IReadableCollaborationsProvider _readableCollaborationsProvider;

    public SearchProfilesManager(ISearchProfilesService searchProfilesService,
        IReadableCollaborationsProvider readableCollaborationsProvider)
    {
        _searchProfilesService = searchProfilesService;
        _readableCollaborationsProvider = readableCollaborationsProvider;
    }

    public async Task<OperationResult<IList<SearchProfileDto>>> SearchAsync(Guid profileId, string searchText)
    {
        var found = await _searchProfilesService.SearchProfilesAsync(searchText);

        var collaborators = await _readableCollaborationsProvider.GetCollaboratedUsersAsync(profileId);

        var alreadyCollaboratedProfiles = GetAlreadyCollaborated(found, collaborators).ToList();
        RemoveAlreadyCollaboratedFromFound(found, alreadyCollaboratedProfiles);

        var alreadyCollaborated = BuildSearchProfileDtos(alreadyCollaboratedProfiles, true);
        var other = BuildSearchProfileDtos(found, false);

        IList<SearchProfileDto> result = alreadyCollaborated
            .Concat(other)
            .Take(1)
            .ToList();

        return OperationResult.FromSuccess(result);
    }

    private static IEnumerable<UserProfile> GetAlreadyCollaborated(IList<UserProfile> foundProfiles,
        IList<UserProfile> collaboratedUsers)
    {
        var foundIds = foundProfiles.Select(profile => profile.Id).ToList();
        return collaboratedUsers.Where(x => foundIds.Contains(x.Id));
    }

    private static void RemoveAlreadyCollaboratedFromFound(IList<UserProfile> found, List<UserProfile> alreadyCollaborated)
    {
        foreach (var userProfile in alreadyCollaborated)
        {
            var profile = found.FirstOrDefault(x => x.Id == userProfile.Id);
            found.Remove(profile!);
        }
    }

    private static IEnumerable<SearchProfileDto> BuildSearchProfileDtos(IEnumerable<UserProfile> profiles, 
        bool isCollaborated) =>
        profiles.Select(profile => new SearchProfileDto
        {
            Id = profile.Id,
            Email = profile.User.Email,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            IsAlreadyCollaborated = isCollaborated
        });
}