using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;

namespace PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

public class UpdateInnoGotchiDto : InnoGotchiRequestDto
{
    public InnoGotchiOperationType InnoGotchiOperationType { get; set; }
}

public enum InnoGotchiOperationType
{
    Feeding,
    Drinking
}