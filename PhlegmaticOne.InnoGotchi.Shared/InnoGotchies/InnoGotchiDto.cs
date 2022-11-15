using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

public class InnoGotchiDto
{
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string HungerLevel { get; set; } = null!;
    public string ThirstLevel { get; set; } = null!;
    public int HappinessDaysCount { get; set; }
    public List<InnoGotchiModelComponentDto> Components { get; set; } = null!;
}