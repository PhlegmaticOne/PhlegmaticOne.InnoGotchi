using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Extensions;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;

public class UpdateInnoGotchiCommand : IdentityOperationResultCommand
{
    public UpdateInnoGotchiCommand(Guid profileId, UpdateInnoGotchiDto updateInnoGotchiDto) : base(profileId) => UpdateInnoGotchiDto = updateInnoGotchiDto;

    public UpdateInnoGotchiDto UpdateInnoGotchiDto { get; set; }
}

public class UpdateInnoGotchiCommandHandler : ValidatableCommandHandler<UpdateInnoGotchiCommand>
{
    private readonly IWritableFarmStatisticsProvider _farmStatisticsProvider;
    private readonly IWritableInnoGotchiesProvider _innoGotchiesProvider;
    private readonly IUnitOfWork _unitOfWork;

    private readonly Dictionary<InnoGotchiOperationType, Func<Guid, UpdateInnoGotchiDto, Task<OperationResult>>>
        _updateOperations;

    public UpdateInnoGotchiCommandHandler(IUnitOfWork unitOfWork,
        IWritableInnoGotchiesProvider innoGotchiesProvider,
        IWritableFarmStatisticsProvider farmStatisticsProvider,
        IValidator<UpdateInnoGotchiCommand> updateCommandValidator) : base(updateCommandValidator)
    {
        _unitOfWork = unitOfWork;
        _innoGotchiesProvider = innoGotchiesProvider;
        _farmStatisticsProvider = farmStatisticsProvider;

        _updateOperations =
            new Dictionary<InnoGotchiOperationType, Func<Guid, UpdateInnoGotchiDto, Task<OperationResult>>>
            {
                { InnoGotchiOperationType.Drinking, DrinkAsync },
                { InnoGotchiOperationType.Feeding, FeedAsync }
            };
    }

    protected override Task<OperationResult> HandleValidCommand(UpdateInnoGotchiCommand request, CancellationToken cancellationToken)
    {
        var operationType = request.UpdateInnoGotchiDto.InnoGotchiOperationType;

        if (_updateOperations.TryGetValue(operationType, out var updateAction))
        {
            return updateAction(request.ProfileId, request.UpdateInnoGotchiDto);
        }

        var result = OperationResult.Failed(AppErrorMessages.UnknownPetOperationType);
        return Task.FromResult(result);
    }

    private Task<OperationResult> DrinkAsync(Guid profileId, UpdateInnoGotchiDto petIdModel)
    {
        return _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.DrinkAsync(petIdModel.PetId);
            await _farmStatisticsProvider.ProcessDrinkingAsync(profileId);
        });
    }

    private Task<OperationResult> FeedAsync(Guid profileId, UpdateInnoGotchiDto petIdModel)
    {
        return _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.FeedAsync(petIdModel.PetId);
            await _farmStatisticsProvider.ProcessFeedingAsync(profileId);
        });
    }
}