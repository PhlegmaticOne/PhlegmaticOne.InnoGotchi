namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;

public class ExistsProfileFarmModel
{
    public ExistsProfileFarmModel(Guid profileId)
    {
        ProfileId = profileId;
    }

    public Guid ProfileId { get; set; }
}