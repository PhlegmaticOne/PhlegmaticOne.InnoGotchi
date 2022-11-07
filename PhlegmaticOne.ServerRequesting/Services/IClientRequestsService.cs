using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.ServerRequesting.Services;

public interface IClientRequestsService
{
    Task<ServerResponse<TResponse>> PostAsync<TResponse>(ClientPostRequest request, string? jwtToken = null);
    Task<ServerResponse<TResponse>> GetAsync<TResponse>(ClientGetRequest request, string? jwtToken = null);
}