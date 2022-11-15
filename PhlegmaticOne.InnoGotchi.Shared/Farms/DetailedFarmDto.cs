using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Shared.Farms;

public class DetailedFarmDto
{
    public string Name { get; set; } = null!;
    public List<InnoGotchiDto> InnoGotchies { get; set; } = new();
}