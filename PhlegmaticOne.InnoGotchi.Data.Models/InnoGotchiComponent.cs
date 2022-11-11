using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class InnoGotchiComponent : EntityBase
{
    public string ImageUrl { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IList<InnoGotchiModelComponent> InnoGotchiModelComponents { get; set; } = new List<InnoGotchiModelComponent>();
}