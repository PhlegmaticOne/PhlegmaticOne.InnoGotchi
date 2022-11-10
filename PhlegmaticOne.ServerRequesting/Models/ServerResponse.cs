using PhlegmaticOne.OperationResults;
using System.Net;

namespace PhlegmaticOne.ServerRequesting.Models;

[Serializable]
public class ServerResponse
{
    public HttpStatusCode StatusCode { get; init; }
    public string ReasonPhrase { get; init; } = string.Empty;
    public bool IsSuccess { get; init; }
    public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;

    public static ServerResponse<T> FromError<T>(HttpStatusCode statusCode, string? reasonPhrase) => new()
    {
        ReasonPhrase = reasonPhrase ?? string.Empty,
        StatusCode = statusCode,
        IsSuccess = false,
        OperationResult = default,
    };

    public static ServerResponse<T> FromSuccess<T>(OperationResult<T> result, HttpStatusCode statusCode, string? reasonPhrase) => new()
    {
        ReasonPhrase = reasonPhrase ?? string.Empty,
        StatusCode = statusCode,
        IsSuccess = true,
        OperationResult = result
    };
}

[Serializable]
public class ServerResponse<TResponse> : ServerResponse
{
    public OperationResult<TResponse>? OperationResult { get; internal set; }
    public TResponse? GetData() => OperationResult is null ? default : OperationResult.Result;
}