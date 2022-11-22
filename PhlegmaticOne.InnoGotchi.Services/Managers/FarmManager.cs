using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class FarmManager : IFarmManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiesProvider;
    private readonly IWritableInnoGotchiesProvider _writableInnoGotchiesProvider;
    private readonly IReadableFarmProvider _readableFarmProvider;
    private readonly IWritableFarmProvider _farmProvider;
    private readonly IReadableCollaborationsProvider _readableCollaborationsProvider;
    private readonly IReadableAvatarProvider _readableAvatarProvider;
    private readonly IMapper _mapper;
    private readonly IValidator<IdentityModel<CreateFarmDto>> _createValidator;

    public FarmManager(IUnitOfWork unitOfWork, 
        IReadableInnoGotchiProvider readableInnoGotchiesProvider,
        IWritableInnoGotchiesProvider writableInnoGotchiesProvider,
        IReadableFarmProvider readableFarmProvider,
        IWritableFarmProvider farmProvider,
        IReadableCollaborationsProvider readableCollaborationsProvider,
        IReadableAvatarProvider readableAvatarProvider,
        IValidator<IdentityModel<CreateFarmDto>> createValidator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _readableInnoGotchiesProvider = readableInnoGotchiesProvider;
        _writableInnoGotchiesProvider = writableInnoGotchiesProvider;
        _readableFarmProvider = readableFarmProvider;
        _farmProvider = farmProvider;
        _readableCollaborationsProvider = readableCollaborationsProvider;
        _readableAvatarProvider = readableAvatarProvider;
        _mapper = mapper;
        _createValidator = createValidator;
    }

    public async Task<OperationResult<DetailedFarmDto>> GetWithPetsAsync(Guid profileId)
    {
        var farmResult = await _readableFarmProvider.GetFarmWithProfileAsync(profileId);
        var farm = farmResult.Result!;

        await _writableInnoGotchiesProvider.SynchronizeSignsAsync(farm.Id);
        await _unitOfWork.SaveChangesAsync();

        var allPets = await _readableInnoGotchiesProvider.GetAllDetailedAsync(farm.Id);
        farm.InnoGotchies = allPets.Result!;

        var mapped = _mapper.Map<DetailedFarmDto>(farm);
        return OperationResult.FromSuccess(mapped);
    }

    public async Task<OperationResult<IList<PreviewFarmDto>>> GetCollaboratedAsync(Guid profileId)
    {
        var collaboratedFarms = await _readableCollaborationsProvider.GetCollaboratedFarmsWithUsersAsync(profileId);

        IList<PreviewFarmDto> result = new List<PreviewFarmDto>();
        foreach (var collaboratedFarm in collaboratedFarms.Result!)
        {
            var avatar = await _readableAvatarProvider.GetAvatarAsync(collaboratedFarm.OwnerId);
            result.Add(new PreviewFarmDto
            {
                Email = collaboratedFarm.Owner.User.Email,
                FirstName = collaboratedFarm.Owner.FirstName,
                LastName = collaboratedFarm.Owner.LastName,
                ProfileId = collaboratedFarm.OwnerId,
                Name = collaboratedFarm.Name,
                OwnerAvatarData = avatar.Result!.AvatarData
            });
        }

        return OperationResult.FromSuccess(result);
    }


    public async Task<OperationResult<DetailedFarmDto>> CreateAsync(IdentityModel<CreateFarmDto> createFarmIdentityModel)
    {
        var validationResult = await _createValidator.ValidateAsync(createFarmIdentityModel);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedFarmDto>(validationResult.ToDictionary());
        }

        var created = await _farmProvider.CreateAsync(createFarmIdentityModel);

        if (created.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedFarmDto>(created.ErrorMessage);
        }

        await _unitOfWork.SaveChangesAsync();

        var mapped = _mapper.Map<DetailedFarmDto>(created.Result);
        return OperationResult.FromSuccess(mapped);
    }
}