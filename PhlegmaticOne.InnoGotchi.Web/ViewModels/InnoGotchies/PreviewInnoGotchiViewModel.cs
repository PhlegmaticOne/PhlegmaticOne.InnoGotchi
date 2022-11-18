using PhlegmaticOne.InnoGotchi.Web.ViewModels.Components;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class PreviewInnoGotchiViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string HungerLevel { get; set; } = null!;
    public string ThirstyLevel { get; set; } = null!;
    public bool IsNewBorn => Age == 0;
    public List<InnoGotchiComponentViewModel> Components { get; set; } = null!;
}