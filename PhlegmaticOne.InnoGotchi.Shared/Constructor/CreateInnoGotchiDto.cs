using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Shared.Constructor;

public class CreateInnoGotchiDto
{
    public string Name { get; set; } = null!;
    public List<InnoGotchiModelComponentDto> Components { get; set; } = null!;
}