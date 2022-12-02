using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Synchronization.Base;

public abstract class SynchronizeFarmCommandHandlerBase<TRequest> : IOperationResultCommandHandler<TRequest>
    where TRequest : IOperationResultCommand
{
    private readonly IInnoGotchiesSynchronizationProvider _innoGotchiesSynchronizationProvider;
    protected readonly IUnitOfWork UnitOfWork;

    protected SynchronizeFarmCommandHandlerBase(IUnitOfWork unitOfWork,
        IInnoGotchiesSynchronizationProvider innoGotchiesSynchronizationProvider)
    {
        UnitOfWork = unitOfWork;
        _innoGotchiesSynchronizationProvider = innoGotchiesSynchronizationProvider;
    }

    public Task<OperationResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return UnitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            var farmId = await GetFarmId(request, cancellationToken);
            await _innoGotchiesSynchronizationProvider
                .SynchronizePetsInFarmAsync(farmId, cancellationToken);
        });
    }

    protected abstract Task<Guid> GetFarmId(TRequest request, CancellationToken cancellationToken);
}