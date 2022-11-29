using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;

public class GetPreviewInnoGotchiRequest : ClientGetRequest<Guid, PreviewInnoGotchiDto>
{
    public GetPreviewInnoGotchiRequest(Guid requestData) : base(requestData) { }

    public override string BuildQueryString() => WithOneQueryParameter(new("petId", RequestData));
}