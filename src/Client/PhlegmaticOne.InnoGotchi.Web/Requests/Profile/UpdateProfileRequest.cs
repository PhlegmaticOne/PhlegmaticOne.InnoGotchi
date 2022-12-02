using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class UpdateProfileRequest : ClientPutRequest<UpdateProfileDto, AuthorizedProfileDto>
{
    public UpdateProfileRequest(UpdateProfileDto requestData) : base(requestData)
    {
    }
}