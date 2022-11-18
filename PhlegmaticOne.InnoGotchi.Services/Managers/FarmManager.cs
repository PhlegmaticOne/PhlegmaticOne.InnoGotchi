using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class FarmManager : IFarmManager
{
    private readonly IFarmProvider _farmProvider;
    private readonly IMapper _mapper;
    private readonly IValidator<IdentityModel<CreateFarmDto>> _createValidator;

    public FarmManager(IFarmProvider farmProvider, IMapper mapper,
        IValidator<IdentityModel<CreateFarmDto>> createValidator)
    {
        _farmProvider = farmProvider;
        _mapper = mapper;
        _createValidator = createValidator;
    }

    public async Task<OperationResult<DetailedFarmDto>> GetWithPetsAsync(Guid profileId)
    {
        var result = await _farmProvider.GetWithPetsAsync(profileId);

        if (result.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedFarmDto>(result.ErrorMessage);
        }

        var mapped = _mapper.Map<DetailedFarmDto>(result.Result!);

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

        var mapped = _mapper.Map<DetailedFarmDto>(created.Result);
        return OperationResult.FromSuccess(mapped);
    }
}