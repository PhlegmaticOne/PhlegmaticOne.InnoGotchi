namespace PhlegmaticOne.ServerRequesting.Models.Requests;

public abstract class ClientPutRequest<TRequest, TResponse> : ClientRequest<TRequest, TResponse>
{
    protected ClientPutRequest(TRequest requestData) : base(requestData) { }
}