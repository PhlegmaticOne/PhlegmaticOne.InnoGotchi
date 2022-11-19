﻿using PhlegmaticOne.OperationResults.Validation;

namespace PhlegmaticOne.OperationResults;

[Serializable]
public class OperationResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public ValidationResult? ValidationResult { get; init; }
    public static OperationResult<T> FromSuccess<T>(T result) => new()
    {
        IsSuccess = true,
        ErrorMessage = null,
        Result = result,
        ValidationResult = ValidationResult.Success
    };

    public static OperationResult Success => new()
    {
        IsSuccess = true,
        ValidationResult = ValidationResult.Success
    };

    public static OperationResult<T> FromFail<T>(string? errorMessage = null) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage ?? "Operation error",
        Result = default,
        ValidationResult = ValidationResult.Error
    };

    public static OperationResult FromFail(string? errorMessage = null) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage ?? "Operation error",
        ValidationResult = ValidationResult.Error
    };

    public static OperationResult<T> FromFail<T>(IDictionary<string, string[]> validationFailures, string? errorMessage = null)
    {
        var validationResult = ValidationResult.FromFailures(validationFailures);
        return new()
        {
            IsSuccess = false,
            Result = default,
            ValidationResult = validationResult,
            ErrorMessage = errorMessage ?? validationResult.OnlyErrors(".")
        };
    }

    public static OperationResult<T> FromFail<T>(ValidationResult validationResult, string? errorMessage = null) => new()
    {
        IsSuccess = false,
        Result = default,
        ValidationResult = validationResult,
        ErrorMessage = errorMessage ?? validationResult.OnlyErrors(".")
    };
}

[Serializable]
public class OperationResult<T> : OperationResult
{
    public T? Result { get; init; }

}