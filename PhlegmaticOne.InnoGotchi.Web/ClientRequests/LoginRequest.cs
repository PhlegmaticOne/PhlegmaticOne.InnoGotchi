using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class LoginRequest : ClientPostRequest<LoginDto>
{
    public LoginRequest(LoginDto requestData) : base(requestData) { }
}