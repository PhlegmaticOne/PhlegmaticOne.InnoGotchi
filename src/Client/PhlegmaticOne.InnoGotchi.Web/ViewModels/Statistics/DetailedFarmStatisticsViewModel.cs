namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Statistics;

public class DetailedFarmStatisticsViewModel
{
    public Guid FarmId { get; set; }
    public string FarmName { get; set; } = null!;
    public int PetsCount { get; set; }
    public int AlivePetsCount { get; set; }
    public int DeadPetsCount { get; set; }
    public TimeSpan AverageFeedingPeriod { get; set; }
    public TimeSpan AverageThirstQuenchingPeriod { get; set; }
    public double AverageHappinessDaysCount { get; set; }
    public double AverageAlivePetsAge { get; set; }
    public double AverageDeadPetsAge { get; set; }
}