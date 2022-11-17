using PhlegmaticOne.InnoGotchi.Web.ViewModels.Components;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class DetailedInnoGotchiViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string HungerLevel { get; set; } = null!;
    public string ThirstLevel { get; set; } = null!;
    public int HappinessDaysCount { get; set; }
    public DateTime LiveSince { get; set; }
    public DateTime DeadSince { get; set; }
    public bool IsDead => DeadSince != DateTime.MinValue;
    public List<InnoGotchiComponentViewModel> Components { get; set; } = null!;
}