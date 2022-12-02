namespace PhlegmaticOne.OperationResults;

[Serializable]
public class OperationResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static OperationResult Success => new()
    {
        IsSuccess = true
    };

    public static OperationResult<T> FromSuccess<T>(T result)
    {
        return new()
        {
            IsSuccess = true,
            ErrorMessage = null,
            Result = result
        };
    }

    public static OperationResult<T> FromFail<T>(string? errorMessage = null)
    {
        return new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage ?? "Operation error",
            Result = default
        };
    }

    public static OperationResult FromFail(string? errorMessage = null)
    {
        return new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage ?? "Operation error"
        };
    }
}

[Serializable]
public class OperationResult<T> : OperationResult
{
    public T? Result { get; init; }
}