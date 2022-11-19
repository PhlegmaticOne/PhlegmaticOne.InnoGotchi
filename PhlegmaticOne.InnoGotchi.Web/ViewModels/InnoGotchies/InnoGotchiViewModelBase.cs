using PhlegmaticOne.InnoGotchi.Web.ViewModels.Components;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class InnoGotchiViewModelBase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string HungerLevel { get; set; } = null!;
    public string ThirstyLevel { get; set; } = null!;
    public bool IsDead { get; set; }
    public bool IsFeedingAllowable { get; set; }
    public bool IsDrinkingAllowable { get; set; }
    public bool IsNewBorn { get; set; }
    public List<InnoGotchiComponentViewModel> Components { get; set; } = null!;
}