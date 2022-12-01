using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class RegisterProfileRequest : ClientPostRequest<RegisterProfileDto, AuthorizedProfileDto>
{
    public RegisterProfileRequest(RegisterProfileDto requestData) : base(requestData) { }
}