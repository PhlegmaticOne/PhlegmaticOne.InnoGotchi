using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class LoginProfileRequest : ClientPostRequest<LoginDto, AuthorizedProfileDto>
{
    public LoginProfileRequest(LoginDto requestData) : base(requestData) { }
}