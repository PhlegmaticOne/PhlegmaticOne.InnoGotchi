using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class TestGetRequest : ClientGetRequest<int>
{
    public TestGetRequest(int requestData) : base(requestData) { }

    public override string BuildQueryString() => 
        WithOneQueryParameter(new("id", RequestData));
}