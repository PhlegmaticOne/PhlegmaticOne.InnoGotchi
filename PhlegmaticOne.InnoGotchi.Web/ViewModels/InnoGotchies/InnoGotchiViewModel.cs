namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class InnoGotchiViewModel
{
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string HungerLevel { get; set; } = null!;
    public string ThirstLevel { get; set; } = null!;
    public int HappinessDaysCount { get; set; }
    public List<InnoGotchiComponentViewModel> Components { get; set; } = null!;
}