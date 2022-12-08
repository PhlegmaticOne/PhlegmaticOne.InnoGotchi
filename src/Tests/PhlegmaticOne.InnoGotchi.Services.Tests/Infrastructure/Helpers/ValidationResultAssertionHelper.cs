using FluentAssertions;
using FluentValidation.Results;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Helpers;

public class ValidationResultAssertionHelper
{
    public static void AssertNotValidWithSingleError(ValidationResult validationResult, string errorMessage)
    {
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should()
            .ContainSingle(f => f.ErrorMessage == errorMessage);
    }

    public static void AssertIsValid(ValidationResult validationResult)
    {
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeEmpty();
    }
}