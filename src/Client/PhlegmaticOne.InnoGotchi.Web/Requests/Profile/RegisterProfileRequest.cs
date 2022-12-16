using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Profile;

public class RegisterProfileRequest : ClientPostRequest<RegisterProfileDto, AuthorizedProfileDto>
{
    public RegisterProfileRequest(RegisterProfileDto requestData) : base(requestData)
    {
    }
}