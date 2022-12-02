using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class InnoGotchiModel : EntityBase
{
    public HungerLevel HungerLevel { get; set; }
    public ThirstyLevel ThirstyLevel { get; set; }
    public DateTime LastFeedTime { get; set; }
    public DateTime LastDrinkTime { get; set; }
    public DateTime AgeUpdatedAt { get; set; }
    public DateTime LiveSince { get; set; }
    public DateTime DeadSince { get; set; }
    public int Age { get; set; }
    public bool IsDead { get; set; }
    public int HappinessDaysCount { get; set; }
    public string Name { get; set; } = null!;
    public Guid FarmId { get; set; }
    public Farm Farm { get; set; } = null!;
    public IList<InnoGotchiModelComponent> Components { get; set; } = new List<InnoGotchiModelComponent>();
}