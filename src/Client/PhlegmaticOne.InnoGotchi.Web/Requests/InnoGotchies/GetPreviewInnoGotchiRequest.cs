using PhlegmaticOne.ApiRequesting.Models;
using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class GetPreviewInnoGotchiRequest : ClientGetRequest<Guid, PreviewInnoGotchiDto>
{
    public GetPreviewInnoGotchiRequest(Guid requestData) : base(requestData)
    {
    }

    public override string BuildQueryString()
    {
        return WithOneQueryParameter(new GetRequestQueryParameter("petId", RequestData));
    }
}