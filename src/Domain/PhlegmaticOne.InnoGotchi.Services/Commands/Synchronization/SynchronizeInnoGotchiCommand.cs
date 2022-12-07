using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Extensions;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Synchronization;

public class SynchronizeInnoGotchiCommand : IOperationResultCommand
{
    public SynchronizeInnoGotchiCommand(Guid petId) => PetId = petId;

    public Guid PetId { get; }
}

public class SynchronizeInnoGotchiCommandHandler :
    IOperationResultCommandHandler<SynchronizeInnoGotchiCommand>
{
    private readonly IInnoGotchiesSynchronizationProvider _innoGotchiesSynchronizationProvider;
    private readonly IUnitOfWork _unitOfWork;

    public SynchronizeInnoGotchiCommandHandler(
        IUnitOfWork unitOfWork,
        IInnoGotchiesSynchronizationProvider innoGotchiesSynchronizationProvider)
    {
        _unitOfWork = unitOfWork;
        _innoGotchiesSynchronizationProvider = innoGotchiesSynchronizationProvider;
    }

    public Task<OperationResult> Handle(SynchronizeInnoGotchiCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesSynchronizationProvider.SynchronizePetAsync(request.PetId, cancellationToken);
        });
}