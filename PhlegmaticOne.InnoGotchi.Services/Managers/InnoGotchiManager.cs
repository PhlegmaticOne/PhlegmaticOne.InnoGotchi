using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PagedLists.Implementation;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class InnoGotchiManager : IInnoGotchiManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiProvider;
    private readonly IWritableInnoGotchiesProvider _innoGotchiesProvider;
    private readonly IValidator<IdentityModel<CreateInnoGotchiDto>> _createValidator;
    private readonly IValidator<IdentityModel<InnoGotchiRequestDto>> _getInnoGotchiValidator;
    private readonly IMapper _mapper;

    public InnoGotchiManager(IUnitOfWork unitOfWork,
        IReadableInnoGotchiProvider readableInnoGotchiProvider,
        IWritableInnoGotchiesProvider innoGotchiesProvider,
        IValidator<IdentityModel<CreateInnoGotchiDto>> createValidator,
        IValidator<IdentityModel<InnoGotchiRequestDto>> getInnoGotchiValidator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _readableInnoGotchiProvider = readableInnoGotchiProvider;
        _innoGotchiesProvider = innoGotchiesProvider;
        _createValidator = createValidator;
        _getInnoGotchiValidator = getInnoGotchiValidator;
        _mapper = mapper;
    }

    public async Task<OperationResult> CreateAsync(
        IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createInnoGotchiDto);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail(validationResult.ToString());
        }

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.CreateAsync(createInnoGotchiDto);
        });
    }

    public async Task<OperationResult<DetailedInnoGotchiDto>> GetDetailedAsync(
        IdentityModel<InnoGotchiRequestDto> petIdModel)
    {
        var validationResult = await _getInnoGotchiValidator.ValidateAsync(petIdModel);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(validationResult.ToString());
        }

        var synchronizationResult = await _unitOfWork.ResultFromExecutionInTransaction(() =>
            _innoGotchiesProvider.SynchronizeSignsAsync(petIdModel.Entity.PetId));

        if (synchronizationResult.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(synchronizationResult.ErrorMessage);
        }

        var petResult = await _readableInnoGotchiProvider.GetDetailedAsync(petIdModel.Entity.PetId);
        var mapped = _mapper.Map<DetailedInnoGotchiDto>(petResult);
        return OperationResult.FromSuccess(mapped);
    }

    public async Task<OperationResult<PreviewInnoGotchiDto>> GetPreviewAsync(Guid petId)
    {
        var result = await _readableInnoGotchiProvider.GetDetailedAsync(petId);
        var mapped = _mapper.Map<PreviewInnoGotchiDto>(result);
        return OperationResult.FromSuccess(mapped);
    }

    public async Task<OperationResult<PagedList<ReadonlyInnoGotchiPreviewDto>>> GetPagedAsync(
        IdentityModel<PagedListData> pagedIdentityModel)
    {
        var pagedListResult = await _readableInnoGotchiProvider
            .GetPagedAsync(pagedIdentityModel.ProfileId, pagedIdentityModel.Entity);
        var mapped = _mapper.Map<PagedList<ReadonlyInnoGotchiPreviewDto>>(pagedListResult);
        return OperationResult.FromSuccess(mapped);
    }
}