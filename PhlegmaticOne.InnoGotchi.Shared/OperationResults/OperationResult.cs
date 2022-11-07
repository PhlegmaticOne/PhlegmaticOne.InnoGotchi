namespace PhlegmaticOne.InnoGotchi.Shared.OperationResults;

[Serializable]
public class OperationResult<T>
{
    public T? Result { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public static OperationResult<T> FromSuccess(T result) => new()
    {
        IsSuccess = true,
        Result = result
    };

    public static OperationResult<T> FromFail(string message, Exception? exception = null) => new()
    {
        Message = message,
        IsSuccess = false
    };
}