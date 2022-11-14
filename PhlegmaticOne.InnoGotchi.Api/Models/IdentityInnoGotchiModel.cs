using PhlegmaticOne.InnoGotchi.Api.Models.Base;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Api.Models;

public class IdentityInnoGotchiModel : IdentityModelBase
{
    public string Name { get; set; } = null!;
    public List<CreateInnoGotchiComponentDto> Components { get; set; } = null!;
}