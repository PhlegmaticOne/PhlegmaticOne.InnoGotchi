using PhlegmaticOne.InnoGotchi.Web.ViewModels.Components;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class DetailedInnoGotchiViewModel
{
    public Guid Id { get; set; }
    public string HungerLevel { get; set; } = null!;
    public string ThirstyLevel { get; set; } = null!;
    public DateTime LastFeedTime { get; set; }
    public DateTime LastDrinkTime { get; set; }
    public DateTime AgeUpdatedAt { get; set; }
    public DateTime LiveSince { get; set; }
    public DateTime DeadSince { get; set; }
    public int Age { get; set; }
    public int HappinessDaysCount { get; set; }
    public string Name { get; set; } = null!;
    public bool IsDead => DeadSince != DateTime.MinValue;
    public bool IsNewBorn => Age == 0;
    public List<InnoGotchiComponentViewModel> Components { get; set; } = null!;
}