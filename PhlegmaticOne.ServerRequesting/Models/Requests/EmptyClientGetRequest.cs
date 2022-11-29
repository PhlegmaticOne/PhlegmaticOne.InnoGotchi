namespace PhlegmaticOne.ServerRequesting.Models.Requests;

public abstract class EmptyClientGetRequest<TResponse> : ClientGetRequest<object, TResponse>
{
    protected EmptyClientGetRequest() : base(default!) => IsEmpty = true;
    public sealed override string BuildQueryString() => string.Empty;
}