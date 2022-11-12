using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class UpdateAccountRequest : ClientPostRequest<UpdateProfileDto, AuthorizedProfileDto>
{
    public UpdateAccountRequest(UpdateProfileDto requestData) : base(requestData) { }
}