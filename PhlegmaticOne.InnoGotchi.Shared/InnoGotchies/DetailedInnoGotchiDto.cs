namespace PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

public class DetailedInnoGotchiDto : InnoGotchiDtoBase
{
    public DateTime LastFeedTime { get; set; }
    public DateTime LastDrinkTime { get; set; }
    public DateTime AgeUpdatedAt { get; set; }
    public DateTime LiveSince { get; set; }
    public DateTime DeadSince { get; set; }
}