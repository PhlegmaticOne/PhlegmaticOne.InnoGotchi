namespace PhlegmaticOne.InnoGotchi.Shared.Farms;

public class FarmStatisticsDto
{
    public string Name { get; set; } = null!;
    public int PetsCount { get; set; }
    public int AlivePetsCount { get; set; }
    public int DeadPetsCount { get; set; }
    public float AverageFeedingPeriod { get; set; }
    public float AverageThirstQuenchingPeriod { get; set; }
    public float AverageHappinessDaysCount { get; set; }
    public float AveragePetsAge { get; set; }
}