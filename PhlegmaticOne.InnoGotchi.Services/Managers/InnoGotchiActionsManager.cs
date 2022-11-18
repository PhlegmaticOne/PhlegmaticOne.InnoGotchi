using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class InnoGotchiActionsManager : IInnoGotchiActionsManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiProvider;
    private readonly IWritableInnoGotchiesProvider _innoGotchiesProvider;
    private readonly IWritableFarmStatisticsProvider _farmStatisticsProvider;
    private readonly IMapper _mapper;

    public InnoGotchiActionsManager(IUnitOfWork unitOfWork,
        IReadableInnoGotchiProvider readableInnoGotchiProvider,
        IWritableInnoGotchiesProvider innoGotchiesProvider,
        IWritableFarmStatisticsProvider farmStatisticsProvider,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _readableInnoGotchiProvider = readableInnoGotchiProvider;
        _innoGotchiesProvider = innoGotchiesProvider;
        _farmStatisticsProvider = farmStatisticsProvider;
        _mapper = mapper;
    }

    public async Task<OperationResult<DetailedInnoGotchiDto>> DrinkAsync(IdentityModel<Guid> petIdModel)
    {
        var drinkResult = await _innoGotchiesProvider.DrinkAsync(petIdModel);

        if (drinkResult.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(drinkResult.ErrorMessage);
        }

        var farmStatisticsResult = await _farmStatisticsProvider.ProcessDrinkingAsync(petIdModel.ProfileId);

        if (farmStatisticsResult.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(farmStatisticsResult.ErrorMessage);
        }

        await _unitOfWork.SaveChangesAsync();

        var innoGotchiResult = await _readableInnoGotchiProvider.GetDetailedAsync(petIdModel.Entity, petIdModel.ProfileId);
        var mapped = _mapper.Map<DetailedInnoGotchiDto>(innoGotchiResult.Result);

        return OperationResult.FromSuccess(mapped);
    }

    public async Task<OperationResult<DetailedInnoGotchiDto>> FeedAsync(IdentityModel<Guid> petIdModel)
    {
        var drinkResult = await _innoGotchiesProvider.FeedAsync(petIdModel);

        if (drinkResult.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(drinkResult.ErrorMessage);
        }

        var farmStatisticsResult = await _farmStatisticsProvider.ProcessFeedingAsync(petIdModel.ProfileId);

        if (farmStatisticsResult.IsSuccess == false)
        {
            return OperationResult.FromFail<DetailedInnoGotchiDto>(farmStatisticsResult.ErrorMessage);
        }

        var innoGotchiResult = await _readableInnoGotchiProvider.GetDetailedAsync(petIdModel.Entity, petIdModel.ProfileId);
        var mapped = _mapper.Map<DetailedInnoGotchiDto>(innoGotchiResult.Result);

        return OperationResult.FromSuccess(mapped);
    }
}