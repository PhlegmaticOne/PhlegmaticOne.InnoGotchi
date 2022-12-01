namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;

public class ExistsProfileFarmModel
{
    public Guid ProfileId { get; set; }

    public ExistsProfileFarmModel(Guid profileId)
    {
        ProfileId = profileId;
    }
}