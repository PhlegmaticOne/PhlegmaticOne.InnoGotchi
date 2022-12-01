using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;

public class UpdateInnoGotchiCommand : IdentityOperationResultCommandBase
{
    public UpdateInnoGotchiDto UpdateInnoGotchiDto { get; }

    public UpdateInnoGotchiCommand(Guid profileId, UpdateInnoGotchiDto updateInnoGotchiDto) : base(profileId) => 
        UpdateInnoGotchiDto = updateInnoGotchiDto;
}

public class UpdateInnoGotchiCommandHandler : IOperationResultCommandHandler<UpdateInnoGotchiCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableInnoGotchiesProvider _innoGotchiesProvider;
    private readonly IWritableFarmStatisticsProvider _farmStatisticsProvider;
    private readonly IValidator<IdentityModel<InnoGotchiRequestDto>> _updateCommandValidator;
    private readonly Dictionary<InnoGotchiOperationType, Func<Guid, UpdateInnoGotchiDto, Task<OperationResult>>> _updateOperations;
    public UpdateInnoGotchiCommandHandler(IUnitOfWork unitOfWork,
        IWritableInnoGotchiesProvider innoGotchiesProvider,
        IWritableFarmStatisticsProvider farmStatisticsProvider,
        IValidator<IdentityModel<InnoGotchiRequestDto>> updateCommandValidator)
    {
        _unitOfWork = unitOfWork;
        _innoGotchiesProvider = innoGotchiesProvider;
        _farmStatisticsProvider = farmStatisticsProvider;
        _updateCommandValidator = updateCommandValidator;

        _updateOperations = new()
        {
            { InnoGotchiOperationType.Drinking, DrinkAsync },
            { InnoGotchiOperationType.Feeding, FeedAsync }
        };
    }

    public async Task<OperationResult> Handle(UpdateInnoGotchiCommand request, CancellationToken cancellationToken)
    {
        var validatingModel = new IdentityModel<InnoGotchiRequestDto>
        {
            ProfileId = request.ProfileId,
            Entity = request.UpdateInnoGotchiDto
        };

        var validationResult = await _updateCommandValidator.ValidateAsync(validatingModel, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail(validationResult.ToString());
        }

        var operationType = request.UpdateInnoGotchiDto.InnoGotchiOperationType;
        if (_updateOperations.TryGetValue(operationType, out var updateAction))
        {
            return await updateAction(request.ProfileId, request.UpdateInnoGotchiDto);
        }

        return OperationResult.FromFail("Unknown operation over InnoGotchi");
    }

    private Task<OperationResult> DrinkAsync(Guid profileId, UpdateInnoGotchiDto petIdModel)
    {
        return _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.DrinkAsync(petIdModel.PetId);
            await _farmStatisticsProvider.ProcessDrinkingAsync(profileId);
        });
    }

    private Task<OperationResult> FeedAsync(Guid profileId, UpdateInnoGotchiDto  petIdModel)
    {
        return _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.FeedAsync(petIdModel.PetId);
            await _farmStatisticsProvider.ProcessFeedingAsync(profileId);
        });
    }
}