using Microsoft.AspNetCore.Mvc.ModelBinding;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;

public static class OperationResultExtensions
{
    public static void AddErrorsToModelState(this OperationResult operationResult,
        ModelStateDictionary modelStateDictionary)
    {
        var validationResult = operationResult.ValidationResult;
        if (validationResult?.IsValid == false)
        {
            foreach (var error in validationResult.ToDictionary())
            {
                modelStateDictionary.TryAddModelError(error.Key, error.Value);
            }
        }
    }
}