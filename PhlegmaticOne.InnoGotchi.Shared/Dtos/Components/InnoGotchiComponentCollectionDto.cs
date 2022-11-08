namespace PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;

public record InnoGotchiComponentCollectionDto(List<InnoGotchiComponentDto> Components)
{
    public InnoGotchiComponentCollectionDto() : this(new List<InnoGotchiComponentDto>())
    {

    }
}