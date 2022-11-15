using FluentValidation;
using FluentValidation.Results;
using PhlegmaticOne.DataService.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Verifying.Base;

public abstract class VerifyingServiceBase<TFrom, TTo> : IVerifyingService<TFrom, TTo>
    where TFrom : class
    where TTo : class
{
    protected readonly IValidator<TFrom> FluentValidator;
    protected readonly IDataService DataService;

    protected VerifyingServiceBase(IValidator<TFrom> fluentValidator, IDataService dataService)
    {
        FluentValidator = fluentValidator;
        DataService = dataService;
    }

    public Task<ValidationResult> ValidateAsync(TFrom from) => 
        FluentValidator.ValidateAsync(from);

    public abstract Task<TTo> MapAsync(TFrom from);
}