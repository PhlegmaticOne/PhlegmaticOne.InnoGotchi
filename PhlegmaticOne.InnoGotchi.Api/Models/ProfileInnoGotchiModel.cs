using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Api.Models;

public class ProfileInnoGotchiModel
{
    public Guid ProfileId { get; set; }
    public string Name { get; set; } = null!;
    public List<ComponentDto> Components { get; set; } = null!;
}