using PhlegmaticOne.InnoGotchi.Shared.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Shared.Components;

public class InnoGotchiComponentDto : IHaveImageUrl
{
    public string ImageUrl { get; set; } = null!;
    public string Name { get; set; } = null!;
}