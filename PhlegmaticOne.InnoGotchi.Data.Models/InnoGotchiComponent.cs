using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class InnoGotchiComponent : ModelBase
{
    public string ImageUrl { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IEnumerable<InnoGotchiModelComponent> InnoGotchiModelComponents { get; set; }
}