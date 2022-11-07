namespace PhlegmaticOne.ServerRequesting.Models;

public class ServerResponse<T>
{
    public int StatusCode { get; init; }
    public string ReasonPhrase { get; init; } = string.Empty;
    public T? ResponseData { get; private set; }
    public bool IsSuccess => ResponseData is not null;

    public static ServerResponse<T> FromSuccess(T data, int statusCode, string reasonPhrase)
    {
        var result = FromError(statusCode, reasonPhrase);
        result.ResponseData = data;

        return result;
    }
    
    public static ServerResponse<T> FromError(int statusCode, string reasonPhrase) => new()
    {
        ReasonPhrase = reasonPhrase ?? string.Empty,
        StatusCode = statusCode
    };
}