using PhlegmaticOne.OperationResults;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.ServerRequesting.Services;

public interface IClientRequestsService
{
    Task<ServerResponse<OperationResult<TResponse>>> PostAsync<TResponse>(ClientPostRequest request, string? jwtToken = null);
    Task<ServerResponse<OperationResult<TResponse>>> GetAsync<TResponse>(ClientGetRequest request, string? jwtToken = null);
}