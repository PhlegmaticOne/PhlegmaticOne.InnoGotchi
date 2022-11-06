using PhlegmaticOne.InnoGotchi.Data.Models.Base;
using PhlegmaticOne.InnoGotchi.Data.Models.Enums;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class InnoGotchiModel : ModelBase
{
    public HungerLevel HungerLevel { get; set; }
    public ThirstyLevel ThirstyLevel { get; set; }
    public string Name { get; set; }
    public IEnumerable<InnoGotchiModelComponent> Components { get; set; }
    public Farm Farm { get; set; }
}