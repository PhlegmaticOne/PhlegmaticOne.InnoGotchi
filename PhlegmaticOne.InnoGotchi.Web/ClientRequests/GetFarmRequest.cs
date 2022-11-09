using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class GetFarmRequest : ClientGetRequest<string>
{
    public GetFarmRequest(string requestData) : base(requestData) { }

    public override string BuildQueryString() => 
        WithOneQueryParameter(new("email", RequestData));
}