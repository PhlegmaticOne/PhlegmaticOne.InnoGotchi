using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class GotchiComponent : ModelBase
{
    public string ImageUrl { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IEnumerable<InnoGotchi> InnoGotchies { get; set; }
}