using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.ServerRequesting.Builders;

public class ClientRequestsBuilder
{
    private readonly Dictionary<Type, string> _requestUrls = new();
    public void ConfigureRequest<T>(string requestUrl) where T : ClientRequest =>
        _requestUrls.Add(typeof(T), requestUrl);
    internal Dictionary<Type, string> Build() => _requestUrls;
}