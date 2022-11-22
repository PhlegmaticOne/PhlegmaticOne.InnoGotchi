namespace PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;

public class PreviewFarmStatisticsDto
{
    public Guid FarmId { get; set; }
    public string FarmName { get; set; } = null!;
    public int PetsCount { get; set; }
}