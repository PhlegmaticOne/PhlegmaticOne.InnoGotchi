using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Shared.Farms;

public class DetailedFarmDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public List<PreviewInnoGotchiDto> InnoGotchies { get; set; } = new();
}