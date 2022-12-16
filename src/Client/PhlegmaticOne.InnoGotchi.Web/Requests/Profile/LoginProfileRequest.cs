using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class LoginProfileRequest : ClientPostRequest<LoginDto, AuthorizedProfileDto>
{
    public LoginProfileRequest(LoginDto requestData) : base(requestData)
    {
    }
}