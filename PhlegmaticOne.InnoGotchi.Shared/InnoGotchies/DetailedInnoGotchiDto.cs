using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

public class DetailedInnoGotchiDto
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
    public List<InnoGotchiModelComponentDto> Components { get; set; } = null!;
}