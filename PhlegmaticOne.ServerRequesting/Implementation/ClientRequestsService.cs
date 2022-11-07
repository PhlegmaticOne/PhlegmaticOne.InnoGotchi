using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;
using PhlegmaticOne.ServerRequesting.Models;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.ServerRequesting.Implementation;

public class ClientRequestsService : IClientRequestsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _httpClientName;
    private readonly Dictionary<Type, string> _requestUrls;
    public ClientRequestsService(IHttpClientFactory httpClientFactory, Dictionary<Type, string> requestUrls, string httpClientName)
    {
        _httpClientFactory = httpClientFactory;
        _httpClientName = httpClientName;
        _requestUrls = requestUrls;
    }
    public async Task<ServerResponse<TResponse>> PostAsync<TResponse>(ClientPostRequest request, string? jwtToken = null)
    {
        var requestUrl = _requestUrls[request.GetType()];
        var requestData = request.Data;

        var httpClient = _httpClientFactory.CreateClient(_httpClientName);
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        var httpResponseMessage = await httpClient.PostAsJsonAsync(requestUrl, requestData);

        var httpStatusCode = (int)httpResponseMessage.StatusCode;
        var reasonPhrase = httpResponseMessage.ReasonPhrase;

        if (httpResponseMessage.IsSuccessStatusCode == false)
        {
            return ServerResponse<TResponse>
                .FromError(httpStatusCode, reasonPhrase);
        }

        var result = await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();
        return ServerResponse<TResponse>.FromSuccess(result!, httpStatusCode, reasonPhrase);
    }

    public async Task<ServerResponse<TResponse>> GetAsync<TResponse>(ClientGetRequest request, string? jwtToken = null)
    {
        var requestUrl = _requestUrls[request.GetType()];
        var requestData = request.Data;

        var httpClient = _httpClientFactory.CreateClient(_httpClientName);
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", jwtToken);

        var requestUri = request.IsEmpty ? requestUrl : requestUrl + "/?" + request.BuildQueryString();

        var httpResponseMessage = await httpClient.GetAsync(requestUri);

        var httpStatusCode = (int)httpResponseMessage.StatusCode;
        var reasonPhrase = httpResponseMessage.ReasonPhrase;

        if (httpResponseMessage.IsSuccessStatusCode == false)
        {
            return ServerResponse<TResponse>.FromError(httpStatusCode, reasonPhrase);
        }

        var result = await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();
        return ServerResponse<TResponse>.FromSuccess(result!, httpStatusCode, reasonPhrase);
    }
}