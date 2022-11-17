using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Shared.Farms;

public class DetailedFarmDto
{
    public string Name { get; set; } = null!;
    public List<PreviewInnoGotchiDto> InnoGotchies { get; set; } = new();
}