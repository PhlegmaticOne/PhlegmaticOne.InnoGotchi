using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class RegisterProfileRequest : ClientPostRequest<RegisterProfileDto, AuthorizedProfileDto>
{
    public RegisterProfileRequest(RegisterProfileDto requestData) : base(requestData) { }
}