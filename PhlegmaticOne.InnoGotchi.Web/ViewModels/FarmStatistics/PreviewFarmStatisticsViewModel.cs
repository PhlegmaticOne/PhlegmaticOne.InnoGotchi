namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.FarmStatistics;

public class PreviewFarmStatisticsViewModel
{
    public Guid FarmId { get; set; }
    public string FarmName { get; set; } = null!;
    public int PetsCount { get; set; }
    public string ProfileEmail { get; set; } = null!;
    public string ProfileFirstName { get; set; } = null!;
    public string ProfileLastName { get; set; } = null!;
}