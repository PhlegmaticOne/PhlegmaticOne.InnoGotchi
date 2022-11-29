using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class InnoGotchiActionsManager : IInnoGotchiActionsManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableInnoGotchiesProvider _innoGotchiesProvider;
    private readonly IWritableFarmStatisticsProvider _farmStatisticsProvider;
    private readonly IValidator<IdentityModel<InnoGotchiRequestDto>> _updateValidator;
    private readonly Dictionary<InnoGotchiOperationType, Func<IdentityModel<UpdateInnoGotchiDto>,Task<OperationResult>>> _updateOperations;
    public InnoGotchiActionsManager(IUnitOfWork unitOfWork,
        IWritableInnoGotchiesProvider innoGotchiesProvider,
        IWritableFarmStatisticsProvider farmStatisticsProvider,
        IValidator<IdentityModel<InnoGotchiRequestDto>> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _innoGotchiesProvider = innoGotchiesProvider;
        _farmStatisticsProvider = farmStatisticsProvider;
        _updateValidator = updateValidator;
        _updateOperations = new()
        {
            { InnoGotchiOperationType.Drinking, DrinkAsync },
            { InnoGotchiOperationType.Feeding, FeedAsync }
        };
    }
    public async Task<OperationResult> UpdateAsync(IdentityModel<UpdateInnoGotchiDto> updatePetModel)
    {
        var model = updatePetModel.Transform(new InnoGotchiRequestDto(updatePetModel.Entity.PetId));
        var validationResult = await _updateValidator.ValidateAsync(model);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail(validationResult.ToString());
        }

        if (_updateOperations.TryGetValue(updatePetModel.Entity.InnoGotchiOperationType, out var updateAction))
        {
            return await updateAction(updatePetModel);
        }

        return OperationResult.FromFail("Unknown operation over InnoGotchi");
    }

    private Task<OperationResult> DrinkAsync(IdentityModel<UpdateInnoGotchiDto> petIdModel)
    {
        return _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.DrinkAsync(petIdModel.Entity.PetId);
            await _farmStatisticsProvider.ProcessDrinkingAsync(petIdModel.ProfileId);
        });
    }

    private Task<OperationResult> FeedAsync(IdentityModel<UpdateInnoGotchiDto> petIdModel)
    {
        return _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.FeedAsync(petIdModel.Entity.PetId);
            await _farmStatisticsProvider.ProcessFeedingAsync(petIdModel.ProfileId);
        });
    }
}