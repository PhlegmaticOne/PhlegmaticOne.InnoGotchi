namespace PhlegmaticOne.ServerRequesting.Models;


public abstract class ClientRequest { }

public abstract class ClientRequest<TRequest, TResponse> : ClientRequest
{
    protected ClientRequest(TRequest requestData) => RequestData = requestData;

    public TRequest RequestData { get; set; }
}

public abstract class ClientPostRequest<TRequest, TResponse> : ClientRequest<TRequest, TResponse>
{
    protected ClientPostRequest(TRequest requestData) : base(requestData) { }
}

public abstract class ClientPutRequest<TRequest, TResponse> : ClientRequest<TRequest, TResponse>
{
    protected ClientPutRequest(TRequest requestData) : base(requestData) { }
}

public abstract class ClientGetRequest<TRequest, TResponse> : ClientRequest<TRequest, TResponse>
{
    protected ClientGetRequest(TRequest requestData) : base(requestData) => RequestData = requestData;
    public bool IsEmpty { get; protected set; }
    public abstract string BuildQueryString();

    protected string WithOneQueryParameter(GetRequestQueryParameter getRequestQueryParameter) =>
        getRequestQueryParameter.BuildQueryPart();
    protected string WithManyQueryParameters(params GetRequestQueryParameter[] queryParameters) =>
        string.Join('&', queryParameters.Select(x => x.BuildQueryPart()));
}

public abstract class EmptyClientGetRequest<TResponse> : ClientGetRequest<object, TResponse>
{
    protected EmptyClientGetRequest() : base(default!) => IsEmpty = true;
    public sealed override string BuildQueryString() => string.Empty;
}