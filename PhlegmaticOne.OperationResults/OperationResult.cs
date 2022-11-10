namespace PhlegmaticOne.OperationResults;

[Serializable]
public class OperationResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public string? CustomMessage { get; init; }

    public static OperationResult<T> FromSuccess<T>(T result, string? customMessage = null) => new()
    {
        IsSuccess = true,
        CustomMessage = customMessage,
        ErrorMessage = null,
        Result = result
    };

    public static OperationResult<T> FromFail<T>(string? errorMessage = null, string? customMessage = null) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage,
        CustomMessage = customMessage,
        Result = default
    };
}

[Serializable]
public class OperationResult<T> : OperationResult
{
    public T? Result { get; init; }

}