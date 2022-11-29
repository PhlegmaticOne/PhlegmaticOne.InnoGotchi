using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class ProfileAuthorizedActionsManager : IProfileAuthorizedActionsManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadableAvatarProvider _readableAvatarProvider;
    private readonly IReadableProfileProvider _readableProfileProvider;
    private readonly IWritableProfilesProvider _profilesProvider;
    private readonly IJwtTokenGenerationService _jwtTokenGenerationService;
    private readonly IValidator<IdentityModel<UpdateProfileDto>> _updateValidator;
    private readonly IMapper _mapper;

    public ProfileAuthorizedActionsManager(IUnitOfWork unitOfWork, 
        IReadableAvatarProvider readableAvatarProvider,
        IReadableProfileProvider readableProfileProvider,
        IWritableProfilesProvider profilesProvider,
        IJwtTokenGenerationService jwtTokenGenerationService,
        IValidator<IdentityModel<UpdateProfileDto>> updateValidator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _readableAvatarProvider = readableAvatarProvider;
        _readableProfileProvider = readableProfileProvider;
        _profilesProvider = profilesProvider;
        _jwtTokenGenerationService = jwtTokenGenerationService;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public async Task<OperationResult<AuthorizedProfileDto>> UpdateAsync(IdentityModel<UpdateProfileDto> updateProfileDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateProfileDto);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<AuthorizedProfileDto>(validationResult.ToString());
        }

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            var updated = await _profilesProvider.UpdateAsync(updateProfileDto);
            await _unitOfWork.SaveChangesAsync();

            var mapped = _mapper.Map<AuthorizedProfileDto>(updated);
            mapped.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(updated);

            return mapped;
        });
    }

    public async Task<OperationResult<DetailedProfileDto>> GetDetailedAsync(Guid profileId)
    {
        var detailed = await _readableProfileProvider.GetExistingOrDefaultAsync(profileId);
        var avatar = await _readableAvatarProvider.GetAvatarAsync(profileId);
        detailed!.Avatar = avatar;

        var mapped = _mapper.Map<DetailedProfileDto>(detailed);
        return OperationResult.FromSuccess(mapped);
    }
}