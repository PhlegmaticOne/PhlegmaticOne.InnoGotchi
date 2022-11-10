using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class RegisterProfileRequest : ClientPostRequest<RegisterProfileDto, ProfileDto>
{
    public RegisterProfileRequest(RegisterProfileDto requestData) : base(requestData) { }
}