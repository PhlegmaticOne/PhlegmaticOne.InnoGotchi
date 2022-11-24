using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PagedLists;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class InnoGotchiManager : IInnoGotchiManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiProvider;
    private readonly IWritableInnoGotchiesProvider _innoGotchiesProvider;
    private readonly IValidator<IdentityModel<CreateInnoGotchiDto>> _createValidator;
    private readonly IMapper _mapper;

    public InnoGotchiManager(IUnitOfWork unitOfWork,
        IReadableInnoGotchiProvider readableInnoGotchiProvider,
        IWritableInnoGotchiesProvider innoGotchiesProvider,
        IValidator<IdentityModel<CreateInnoGotchiDto>> createValidator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _readableInnoGotchiProvider = readableInnoGotchiProvider;
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

        await _unitOfWork.SaveChangesAsync();

        var mapped = _mapper.Map<DetailedInnoGotchiDto>(created.Result);
        return OperationResult.FromSuccess(mapped);
    }

    public async Task<OperationResult<DetailedInnoGotchiDto>> GetDetailedAsync(IdentityModel<IdDto> petIdModel)
    {
        await _innoGotchiesProvider.SynchronizeSignsAsync(petIdModel);
        await _unitOfWork.SaveChangesAsync();

        var petResult = await _readableInnoGotchiProvider.GetDetailedAsync(petIdModel.Entity.Id, petIdModel.ProfileId);

        if (petResult.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(petResult.ErrorMessage);
        }

        var mapped = _mapper.Map<DetailedInnoGotchiDto>(petResult.Result);
        return OperationResult.FromSuccess(mapped);
    }

    public async Task<OperationResult<PagedList<ReadonlyInnoGotchiPreviewDto>>> GetPagedAsync(Guid profileId, PagedListData pagedListData)
    {
        var pagedListResult = await _readableInnoGotchiProvider.GetPagedAsync(profileId, pagedListData);
        var mapped = _mapper.Map<PagedList<ReadonlyInnoGotchiPreviewDto>>(pagedListResult.Result!);
        return OperationResult.FromSuccess(mapped);
    }
}