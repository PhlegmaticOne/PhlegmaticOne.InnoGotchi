using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.ServerRequesting.Services;

public interface IClientRequestsService
{
    Task<ServerResponse<TResponse>> PostAsync<TRequest, TResponse>(
        ClientPostRequest<TRequest, TResponse> request, string? jwtToken = null);
    Task<ServerResponse<TResponse>> GetAsync<TRequest, TResponse>(
        ClientGetRequest<TRequest, TResponse> request, string? jwtToken = null);
}