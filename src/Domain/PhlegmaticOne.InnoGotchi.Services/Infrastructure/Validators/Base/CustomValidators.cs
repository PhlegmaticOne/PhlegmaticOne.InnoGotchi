using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators.Base;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, Guid> ProfileMustHaveFarm<T>(
        this IRuleBuilder<T, Guid> ruleBuilder,
        IUnitOfWork unitOfWork,
        string message = AppErrorMessages.FarmDoesNotExistMessage)
    {
        var farmsRepository = unitOfWork.GetRepository<Farm>();
        return ruleBuilder
            .MustAsync((f, ct) => farmsRepository.ExistsAsync(x => x.OwnerId == f, ct))
            .WithMessage(message);
    }

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder,
        int minimumLength = 14)
    {
        var options = ruleBuilder
            .NotEmpty().WithMessage("PasswordEmpty")
            .MinimumLength(minimumLength).WithMessage("PasswordLength")
            .Matches("[A-Z]").WithMessage("PasswordUppercaseLetter")
            .Matches("[a-z]").WithMessage("PasswordLowercaseLetter")
            .Matches("[0-9]").WithMessage("PasswordDigit")
            .Matches("[^a-zA-Z0-9]").WithMessage("PasswordSpecialCharacter");
        return options;
    }
}