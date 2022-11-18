using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class InnoGotchiManager : IInnoGotchiesManager
{
    private readonly IInnoGotchiesProvider _innoGotchiesProvider;
    private readonly IValidator<IdentityModel<CreateInnoGotchiDto>> _createValidator;
    private readonly IMapper _mapper;

    public InnoGotchiManager(IInnoGotchiesProvider innoGotchiesProvider,
        IValidator<IdentityModel<CreateInnoGotchiDto>> createValidator,
        IMapper mapper)
    {
        _innoGotchiesProvider = innoGotchiesProvider;
        _createValidator = createValidator;
        _mapper = mapper;
    }

    public async Task<OperationResult<DetailedInnoGotchiDto>> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createInnoGotchiDto);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(validationResult.ToDictionary());
        }

        var created = await _innoGotchiesProvider.CreateAsync(createInnoGotchiDto);

        if (created.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(created.ErrorMessage);
        }

        var mapped = _mapper.Map<DetailedInnoGotchiDto>(created.Result);
        return OperationResult.FromSuccess(mapped);
    }

    public async Task<OperationResult<DetailedInnoGotchiDto>> GetDetailedAsync(IdentityModel<Guid> petIdModel)
    {
        var result = await _innoGotchiesProvider.GetDetailedAsync(petIdModel);

        if (result.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(result.ErrorMessage);
        }

        var mapped = _mapper.Map<DetailedInnoGotchiDto>(result.Result);

        return OperationResult.FromSuccess(mapped);
    }
}