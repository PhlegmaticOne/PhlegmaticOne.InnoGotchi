using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class UpdateProfileRequest : ClientPostRequest<UpdateProfileDto, AuthorizedProfileDto>
{
    public UpdateProfileRequest(UpdateProfileDto requestData) : base(requestData) { }
}