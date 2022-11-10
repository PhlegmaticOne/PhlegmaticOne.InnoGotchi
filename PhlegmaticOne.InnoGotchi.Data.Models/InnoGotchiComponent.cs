using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class InnoGotchiComponent : EntityBase
{
    public string ImageUrl { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IEnumerable<InnoGotchiModelComponent> InnoGotchiModelComponents { get; set; }
}