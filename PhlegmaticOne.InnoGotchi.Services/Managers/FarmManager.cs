using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
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
    private readonly IValidator<GetFarmModel> _getFarmValidator;
    private readonly IValidator<IdentityModel<GetFarmModel>> _getCollaboratedFarmValidator;

    public FarmManager(IUnitOfWork unitOfWork, 
        IReadableInnoGotchiProvider readableInnoGotchiesProvider,
        IWritableInnoGotchiesProvider writableInnoGotchiesProvider,
        IReadableFarmProvider readableFarmProvider,
        IWritableFarmProvider farmProvider,
        IReadableCollaborationsProvider readableCollaborationsProvider,
        IReadableAvatarProvider readableAvatarProvider,
        IValidator<IdentityModel<CreateFarmDto>> createValidator,
        IValidator<GetFarmModel> getFarmValidator,
        IValidator<IdentityModel<GetFarmModel>> getCollaboratedFarmValidator,
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
        _getFarmValidator = getFarmValidator;
        _getCollaboratedFarmValidator = getCollaboratedFarmValidator;
    }

    public async Task<OperationResult<DetailedFarmDto>> GetWithPetsAsync(Guid profileId)
    {
        var model = new GetFarmModel(profileId);

        var validationResult = await _getFarmValidator.ValidateAsync(model);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedFarmDto>(validationResult.ToString());
        }

        return await GetFarm(profileId);
    }

    public async Task<OperationResult<DetailedFarmDto>> GetCollaboratedFarmWithPetsAsync(
        IdentityModel<Guid> profileIdModel)
    {
        var model = profileIdModel.Transform(new GetFarmModel(profileIdModel.Entity));

        var validationResult = await _getCollaboratedFarmValidator.ValidateAsync(model);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedFarmDto>(validationResult.ToString());
        }

        return await GetFarm(profileIdModel.Entity);
    }

    public async Task<OperationResult<IList<PreviewFarmDto>>> GetCollaboratedAsync(Guid profileId)
    {
        var collaboratedFarms = await _readableCollaborationsProvider
            .GetCollaboratedFarmsWithUsersAsync(profileId);

        IList<PreviewFarmDto> result = new List<PreviewFarmDto>();
        foreach (var collaboratedFarm in collaboratedFarms)
        {
            var avatar = await _readableAvatarProvider.GetAvatarAsync(collaboratedFarm.Owner.Id);
            result.Add(new PreviewFarmDto
            {
                Email = collaboratedFarm.Owner.User.Email,
                FirstName = collaboratedFarm.Owner.FirstName,
                LastName = collaboratedFarm.Owner.LastName,
                ProfileId = collaboratedFarm.Owner.Id,
                Name = collaboratedFarm.Name,
                OwnerAvatarData = avatar.AvatarData
            });
        }

        return OperationResult.FromSuccess(result);
    }

    public async Task<OperationResult<bool>> IsExistsForProfileAsync(Guid profileId)
    {
        var result = await _unitOfWork.GetRepository<Farm>().ExistsAsync(x => x.Owner.Id == profileId);
        return OperationResult.FromSuccess(result);
    }

    public async Task<OperationResult<DetailedFarmDto>> CreateAsync(IdentityModel<CreateFarmDto> createFarmIdentityModel)
    {
        var validationResult = await _createValidator.ValidateAsync(createFarmIdentityModel);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedFarmDto>(validationResult.ToDictionary());
        }

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            var created = await _farmProvider.CreateAsync(createFarmIdentityModel);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DetailedFarmDto>(created);
        });
    }

    private async Task<OperationResult<DetailedFarmDto>> GetFarm(Guid profileId)
    {
        var farm = await _readableFarmProvider.GetFarmWithProfileAsync(profileId);

        await _writableInnoGotchiesProvider.SynchronizeSignsAsync(farm.Id);
        await _unitOfWork.SaveChangesAsync();

        var allPets = await _readableInnoGotchiesProvider.GetAllDetailedAsync(farm.Id);
        farm.InnoGotchies = allPets;

        var mapped = _mapper.Map<DetailedFarmDto>(farm);
        return OperationResult.FromSuccess(mapped);
    }
}