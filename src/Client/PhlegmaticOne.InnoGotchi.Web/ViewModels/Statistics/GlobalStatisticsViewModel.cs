namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Statistics;

public class GlobalStatisticsViewModel
{
    public int ProfilesCount { get; set; }
    public int FarmsCount { get; set; }
    public int PetsTotalCount { get; set; }
    public int DeadPetsCount { get; set; }
    public int AlivePetsCount { get; set; }
    public double AlivePetsAverageAge { get; set; }
    public double DeadPetsAverageAge { get; set; }
    public int CollaborationsCount { get; set; }
    public int PetMaxAge { get; set; }
    public int PetMaxHappinessDaysCount { get; set; }
    public double AverageDaysHappinessCount { get; set; }
}