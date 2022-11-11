using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class LoginRequest : ClientPostRequest<LoginDto, AuthorizedProfileDto>
{
    public LoginRequest(LoginDto requestData) : base(requestData) { }
}