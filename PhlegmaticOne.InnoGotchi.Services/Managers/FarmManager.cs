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
    private readonly IMapper _mapper;
    private readonly IValidator<IdentityModel<CreateFarmDto>> _createValidator;

    public FarmManager(IUnitOfWork unitOfWork, 
        IReadableInnoGotchiProvider readableInnoGotchiesProvider,
        IWritableInnoGotchiesProvider writableInnoGotchiesProvider,
        IReadableFarmProvider readableFarmProvider,
        IWritableFarmProvider farmProvider,
        IValidator<IdentityModel<CreateFarmDto>> createValidator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _readableInnoGotchiesProvider = readableInnoGotchiesProvider;
        _writableInnoGotchiesProvider = writableInnoGotchiesProvider;
        _readableFarmProvider = readableFarmProvider;
        _farmProvider = farmProvider;
        _mapper = mapper;
        _createValidator = createValidator;
    }

    public async Task<OperationResult<DetailedFarmDto>> GetWithPetsAsync(Guid profileId)
    {
        var farmResult = await _readableFarmProvider.GetFarmAsync(profileId);
        var farm = farmResult.Result!;
        var innoGotchiIdsResult = await _readableInnoGotchiesProvider.GetAllIds(farm.Id);
        var innoGotchiIds = innoGotchiIdsResult.Result!;

        //TODO: update collection instead of updating each element

        foreach (var innoGotchiId in innoGotchiIds)
        {
            await _writableInnoGotchiesProvider.SynchronizeSignsAsync(new IdentityModel<Guid>
            {
                Entity = innoGotchiId,
                ProfileId = profileId
            });
        }

        await _unitOfWork.SaveChangesAsync();


        var allPets = await _readableInnoGotchiesProvider.GetAllDetailedAsync(farm.Id);
        farm.InnoGotchies = allPets.Result!;

        var mapped = _mapper.Map<DetailedFarmDto>(farm);
        return OperationResult.FromSuccess(mapped);
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