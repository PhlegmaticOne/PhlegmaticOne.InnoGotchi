namespace PhlegmaticOne.ServerRequesting.Models.Requests;

public abstract class ClientDeleteRequest<TRequest, TResponse> : ClientQueryBuildableRequest<TRequest, TResponse>
{
    protected ClientDeleteRequest(TRequest requestData) : base(requestData) { }
}