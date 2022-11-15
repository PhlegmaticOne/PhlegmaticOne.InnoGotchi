using PhlegmaticOne.DataService.Models;
using PhlegmaticOne.InnoGotchi.Data.Models.Enums;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class InnoGotchiModel : EntityBase
{
    public HungerLevel HungerLevel { get; set; }
    public ThirstyLevel ThirstyLevel { get; set; }
    public DateTime LastFeedTime { get; set; }
    public DateTime LastDrinkTime { get; set; }
    public DateTime AgeUpdatedAt { get; set; }
    public int Age { get; set; }
    public int HappinessDaysCount { get; set; }
    public string Name { get; set; } = null!;
    public Farm Farm { get; set; } = null!;
    public IList<InnoGotchiModelComponent> Components { get; set; } = new List<InnoGotchiModelComponent>();
}