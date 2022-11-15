using PhlegmaticOne.InnoGotchi.Api.Models.Base;
using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Api.Models;

public class IdentityInnoGotchiModel : IdentityModelBase
{
    public string Name { get; set; } = null!;
    public List<InnoGotchiModelComponentDto> Components { get; set; } = null!;
}