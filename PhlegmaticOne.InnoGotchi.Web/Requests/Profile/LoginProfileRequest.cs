using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class LoginProfileRequest : ClientPostRequest<LoginDto, AuthorizedProfileDto>
{
    public LoginProfileRequest(LoginDto requestData) : base(requestData) { }
}