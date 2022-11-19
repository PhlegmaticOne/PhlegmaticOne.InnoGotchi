namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class DetailedInnoGotchiViewModel : InnoGotchiViewModelBase
{
    public DateTime LastFeedTime { get; set; }
    public DateTime LastDrinkTime { get; set; }
    public DateTime AgeUpdatedAt { get; set; }
    public DateTime LiveSince { get; set; }
    public DateTime DeadSince { get; set; }
    public int HappinessDaysCount { get; set; }
}