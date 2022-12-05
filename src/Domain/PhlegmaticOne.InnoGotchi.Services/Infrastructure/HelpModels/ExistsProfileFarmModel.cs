namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;

public class ProfileFarmModel
{
    public ProfileFarmModel(Guid profileId) => ProfileId = profileId;

    public Guid ProfileId { get; set; }
}