using System.Net;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.ServerRequesting.Models;

[Serializable]
public class ServerResponse
{
    public HttpStatusCode StatusCode { get; init; }
    public string ReasonPhrase { get; init; } = string.Empty;
    public bool IsSuccess { get; init; }
    public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;

    public static ServerResponse<OperationResult<T>> FromError<T>(HttpStatusCode statusCode, string? reasonPhrase) => new()
    {
        ReasonPhrase = reasonPhrase ?? string.Empty,
        StatusCode = statusCode,
        IsSuccess = false,
        ResponseData = default
    };

    public static ServerResponse<OperationResult<T>> FromSuccess<T>(OperationResult<T> result, HttpStatusCode statusCode, string? reasonPhrase) => new()
    {
        ReasonPhrase = reasonPhrase ?? string.Empty,
        StatusCode = statusCode,
        IsSuccess = true,
        ResponseData = result
    };
}

[Serializable]
public class ServerResponse<T> : ServerResponse
{
    public T? ResponseData { get; internal set; }

    public bool TryGetData<TData>(out TData? data)
    {
        data = default;
        if (ResponseData is not OperationResult<TData> operationResult) return false;

        data = operationResult.Result;
        return true;
    }

    public TData? GetData<TData>()
    {
        if (ResponseData is OperationResult<TData> operationResult)
        {
            return operationResult.Result;
        }

        throw new ArgumentException($"Response data is not of type OperationResult<{typeof(TData).Name}>");
    }
}