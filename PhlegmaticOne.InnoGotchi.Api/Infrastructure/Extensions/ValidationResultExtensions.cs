using FluentValidation.Results;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;

public static class ValidationResultExtensions
{
    public static string OnlyErrors(this ValidationResult validationResult, string separator = "\n") => 
        string.Join(separator, validationResult.Errors);
}