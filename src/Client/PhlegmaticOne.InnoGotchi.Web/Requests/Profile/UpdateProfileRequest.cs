using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class UpdateProfileRequest : ClientPutRequest<UpdateProfileDto, AuthorizedProfileDto>
{
    public UpdateProfileRequest(UpdateProfileDto requestData) : base(requestData)
    {
    }
}