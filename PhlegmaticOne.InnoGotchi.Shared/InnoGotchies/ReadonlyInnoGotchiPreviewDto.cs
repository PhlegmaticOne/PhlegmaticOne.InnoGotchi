using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

public class ReadonlyInnoGotchiPreviewDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string HungerLevel { get; set; } = null!;
    public string ThirstyLevel { get; set; } = null!;
    public bool IsDead { get; set; }
    public bool IsNewBorn { get; set; }
    public string ProfileFarmName { get; set; } = null!;
    public string ProfileFirstName { get; set; } = null!;
    public string ProfileLastName { get; set; } = null!;
    public List<InnoGotchiModelComponentDto> Components { get; set; } = null!;
}