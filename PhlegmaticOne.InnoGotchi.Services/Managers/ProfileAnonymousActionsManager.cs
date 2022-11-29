using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class ProfileAnonymousActionsManager : IProfileAnonymousActionsManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadableProfileProvider _readableProfileProvider;
    private readonly IWritableProfilesProvider _profilesProvider;
    private readonly IJwtTokenGenerationService _jwtTokenGenerationService;
    private readonly IValidator<RegisterProfileDto> _registerValidator;
    private readonly IMapper _mapper;

    public ProfileAnonymousActionsManager(IUnitOfWork unitOfWork,
        IReadableProfileProvider readableProfileProvider,
        IWritableProfilesProvider profilesProvider,
        IJwtTokenGenerationService jwtTokenGenerationService,
        IValidator<RegisterProfileDto> registerValidator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _readableProfileProvider = readableProfileProvider;
        _profilesProvider = profilesProvider;
        _jwtTokenGenerationService = jwtTokenGenerationService;
        _registerValidator = registerValidator;
        _mapper = mapper;
    }

    public async Task<OperationResult<AuthorizedProfileDto>> RegisterAsync(RegisterProfileDto registerProfileDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerProfileDto);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<AuthorizedProfileDto>(validationResult.ToString());
        }

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            var createdProfile = await _profilesProvider.CreateAsync(registerProfileDto);


            var result = _mapper.Map<AuthorizedProfileDto>(createdProfile);
            result.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(createdProfile);

            return result;
        });
    }

    public async Task<OperationResult<AuthorizedProfileDto>> LoginAsync(LoginDto loginDto)
    {
        var existing = await _readableProfileProvider
            .GetExistingOrDefaultAsync(loginDto.Email, loginDto.Password);

        if (existing is null)
        {
            var message = $"There is not user with email {loginDto.Email} or password is incorrect";
            return OperationResult.FromFail<AuthorizedProfileDto>(message);
        }

        var result = _mapper.Map<AuthorizedProfileDto>(existing);
        result.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(existing);
        return OperationResult.FromSuccess(result);
    }
}