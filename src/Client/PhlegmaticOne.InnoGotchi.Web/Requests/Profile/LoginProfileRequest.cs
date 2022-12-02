using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class LoginProfileRequest : ClientPostRequest<LoginDto, AuthorizedProfileDto>
{
    public LoginProfileRequest(LoginDto requestData) : base(requestData)
    {
    }
}