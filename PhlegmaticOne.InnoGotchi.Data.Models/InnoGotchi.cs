using PhlegmaticOne.InnoGotchi.Data.Models.Base;
using PhlegmaticOne.InnoGotchi.Data.Models.Enums;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class InnoGotchi : ModelBase
{
    public HungerLevel HungerLevel { get; set; }
    public ThirstyLevel ThirstyLevel { get; set; }
    public string Name { get; set; }
    public IEnumerable<GotchiComponent> Components { get; set; }
    public Farm Farm { get; set; }
    public IDictionary<string, string> ToDictionaryByComponentNames() => 
        Components.ToDictionary(x => x.Name, x => x.ImageUrl);
}