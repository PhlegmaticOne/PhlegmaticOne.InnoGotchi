using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;

namespace PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

public class ReadonlyInnoGotchiPreviewDto : InnoGotchiDtoBase
{
    public string ProfileFarmName { get; set; } = null!;
    public string ProfileFirstName { get; set; } = null!;
    public string ProfileLastName { get; set; } = null!;
}