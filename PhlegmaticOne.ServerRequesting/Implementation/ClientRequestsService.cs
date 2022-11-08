﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.ServerRequesting.Models;
using PhlegmaticOne.ServerRequesting.Services;

namespace PhlegmaticOne.ServerRequesting.Implementation;

public class ClientRequestsService : IClientRequestsService
{
    private const string PreQueryPart = "/?";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _httpClientName;
    private readonly Dictionary<Type, string> _requestUrls;
    public ClientRequestsService(IHttpClientFactory httpClientFactory, 
        Dictionary<Type, string> requestUrls,
        string httpClientName)
    {
        _httpClientFactory = httpClientFactory;
        _httpClientName = httpClientName;
        _requestUrls = requestUrls;
    }

    public async Task<ServerResponse<OperationResult<TResponse>>> PostAsync<TResponse>(ClientPostRequest postRequest, string? jwtToken = null)
    {
        var requestUrl = GetRequestUrl(postRequest);
        var httpClient = CreateHttpClientWithToken(jwtToken);
        
        var httpResponseMessage = await httpClient.PostAsJsonAsync(requestUrl, postRequest.Data);

        return await GetServerResponse<TResponse>(httpResponseMessage);
    }

    public async Task<ServerResponse<OperationResult<TResponse>>> GetAsync<TResponse>(ClientGetRequest getRequest, string? jwtToken = null)
    {
        var requestUrl = BuildGetQuery(getRequest);
        var httpClient = CreateHttpClientWithToken(jwtToken);

        var httpResponseMessage = await httpClient.GetAsync(requestUrl);

        return await GetServerResponse<TResponse>(httpResponseMessage);
    }

    private HttpClient CreateHttpClientWithToken(string jwtToken)
    {
        var httpClient = _httpClientFactory.CreateClient(_httpClientName);
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(Constants.BearerAuthenticationSchemeName, jwtToken);
        return httpClient;
    }

    private string BuildGetQuery(ClientGetRequest clientGetRequest)
    {
        var requestUrl = GetRequestUrl(clientGetRequest);
        return clientGetRequest.IsEmpty == false ?
            string.Concat(requestUrl, PreQueryPart, clientGetRequest.BuildQueryString()) :
            requestUrl;
    }

    private string GetRequestUrl(ClientRequest clientRequest) => _requestUrls[clientRequest.GetType()];

    private static async Task<ServerResponse<OperationResult<TResponse>>> GetServerResponse<TResponse>(HttpResponseMessage httpResponseMessage)
    {
        var httpStatusCode = httpResponseMessage.StatusCode;
        var reasonPhrase = httpResponseMessage.ReasonPhrase;

        if (httpResponseMessage.IsSuccessStatusCode == false)
        {
            return ServerResponse.FromError<TResponse>(httpStatusCode, reasonPhrase);
        }

        var result = await httpResponseMessage.Content.ReadFromJsonAsync<OperationResult<TResponse>>();
        return ServerResponse.FromSuccess(result!, httpStatusCode, reasonPhrase);
    }
}