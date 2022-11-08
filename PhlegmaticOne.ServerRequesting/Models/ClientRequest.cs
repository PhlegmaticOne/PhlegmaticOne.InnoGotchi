namespace PhlegmaticOne.ServerRequesting.Models;

public interface IHaveRequestData<out T>
{
    T RequestData { get; }
}

public abstract class ClientRequest
{
    public object Data { get; set; }
    protected ClientRequest(object data) => Data = data;
}

public abstract class ClientPostRequest : ClientRequest
{
    protected ClientPostRequest(object data) : base(data) { }
}


public abstract class ClientPostRequest<T> : ClientPostRequest, IHaveRequestData<T>
{
    protected ClientPostRequest(T requestData) : base(requestData) => RequestData = requestData;
    public T RequestData { get; }
}

public abstract class ClientGetRequest : ClientRequest
{
    public bool IsEmpty { get; protected set; }
    protected ClientGetRequest(object data) : base(data) { }
    public abstract string BuildQueryString();

    protected string WithOneQueryParameter(GetRequestQueryParameter getRequestQueryParameter) => 
        getRequestQueryParameter.BuildQueryPart();

    protected string WithManyQueryParameters(params GetRequestQueryParameter[] queryParameters) => 
        string.Join('&', queryParameters.Select(x => x.BuildQueryPart()));
}
public abstract class ClientGetRequest<T> : ClientGetRequest, IHaveRequestData<T>
{
    protected ClientGetRequest(T requestData) : base(requestData) => RequestData = requestData;
    public T RequestData { get; }
}

public abstract class EmptyClientGetRequest : ClientGetRequest<object>
{
    protected EmptyClientGetRequest() : base(null) => IsEmpty = true;
    public sealed override string BuildQueryString() => string.Empty;
}