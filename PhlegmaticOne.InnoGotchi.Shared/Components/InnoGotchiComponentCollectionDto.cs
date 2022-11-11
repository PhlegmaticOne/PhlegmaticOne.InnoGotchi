namespace PhlegmaticOne.InnoGotchi.Shared.Components;

public record InnoGotchiComponentCollectionDto(List<InnoGotchiComponentDto> Components)
{
    public InnoGotchiComponentCollectionDto() : this(new List<InnoGotchiComponentDto>())
    {

    }
}