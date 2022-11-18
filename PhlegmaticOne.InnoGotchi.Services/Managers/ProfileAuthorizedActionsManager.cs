using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class ProfileAuthorizedActionsManager : IProfileAuthorizedActionsManager
{
    private readonly IProfilesProvider _profilesProvider;
    private readonly IAvatarsProvider _avatarsProvider;
    private readonly IJwtTokenGenerationService _jwtTokenGenerationService;
    private readonly IValidator<UpdateProfileDto> _updateValidator;
    private readonly IMapper _mapper;

    public ProfileAuthorizedActionsManager(IProfilesProvider profilesProvider,
        IAvatarsProvider avatarsProvider,
        IJwtTokenGenerationService jwtTokenGenerationService,
        IValidator<UpdateProfileDto> updateValidator,
        IMapper mapper)
    {
        _profilesProvider = profilesProvider;
        _avatarsProvider = avatarsProvider;
        _jwtTokenGenerationService = jwtTokenGenerationService;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }
    public async Task<OperationResult<AuthorizedProfileDto>> UpdateAsync(UpdateProfileDto updateProfileDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateProfileDto);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<AuthorizedProfileDto>(validationResult.ToDictionary());
        }

        var updatedOperationResult = await _profilesProvider.UpdateAsync(updateProfileDto);
        var updated = updatedOperationResult.Result!;

        var mapped = _mapper.Map<AuthorizedProfileDto>(updated);
        mapped.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(updated);

        return OperationResult.FromSuccess(mapped);
    }

    public async Task<OperationResult<DetailedProfileDto>> GetDetailedAsync(Guid profileId)
    {
        var detailedOperationResult = await _profilesProvider.GetExistingOrDefaultAsync(profileId);
        var avatar = await _avatarsProvider.GetAvatarAsync(profileId);

        var detailed = detailedOperationResult.Result!;
        detailed.Avatar = avatar.Result;

        var mapped = _mapper.Map<DetailedProfileDto>(detailed);
        return OperationResult.FromSuccess(mapped);
    }
}