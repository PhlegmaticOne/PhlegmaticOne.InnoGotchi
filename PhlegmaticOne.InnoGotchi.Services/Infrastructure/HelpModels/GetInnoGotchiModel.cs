using PhlegmaticOne.InnoGotchi.Domain.Identity;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;

public class GetInnoGotchiModel : IHaveProfileId
{
    public Guid ProfileId { get; set; }

    public GetInnoGotchiModel(Guid profileId) => ProfileId = profileId;
}