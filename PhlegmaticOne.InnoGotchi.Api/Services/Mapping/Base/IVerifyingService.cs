using FluentValidation.Results;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;

public interface IVerifyingService<in TFrom, TTo>
    where TFrom : class
    where TTo : class
{
    Task<ValidationResult> ValidateAsync(TFrom from);
    Task<TTo> MapAsync(TFrom from);
}