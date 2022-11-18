using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class FarmStatistics : EntityBase
{
    public DateTime LastFeedTime { get; set; }
    public int TotalFeedingsCount { get; set; }
    public TimeSpan AverageFeedTime { get; set; }
    public DateTime LastDrinkTime { get; set; }
    public int TotalDrinkingsCount { get; set; }
    public TimeSpan AverageDrinkTime { get; set; }
    public Guid FarmId { get; set; }
    public Farm Farm { get; set; } = null!;
}