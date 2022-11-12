using FluentValidation;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 14)
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