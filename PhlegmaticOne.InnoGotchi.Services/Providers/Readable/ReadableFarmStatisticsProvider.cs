using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableFarmStatisticsProvider : IReadableFarmStatisticsProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadableFarmStatisticsProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OperationResult<FarmStatistics>> GetForFarmAsync(Guid farmId)
    {
        var repository = _unitOfWork.GetDataRepository<FarmStatistics>();
        var statistics = await repository.GetFirstOrDefaultAsync(
            predicate: p => p.FarmId == farmId);
        return OperationResult.FromSuccess(statistics!);
    }
}