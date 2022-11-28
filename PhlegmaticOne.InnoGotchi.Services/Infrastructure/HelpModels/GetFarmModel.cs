using PhlegmaticOne.InnoGotchi.Domain.Identity;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;

public class GetFarmModel : IHaveProfileId
{
    public Guid ProfileId { get; set; }

    public GetFarmModel(Guid profileId)
    {
        ProfileId = profileId;
    }
}