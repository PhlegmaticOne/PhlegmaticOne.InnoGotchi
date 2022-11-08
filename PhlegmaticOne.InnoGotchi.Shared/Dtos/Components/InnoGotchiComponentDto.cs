namespace PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;

public record InnoGotchiComponentDto(string ImageUrl, string Name)
{
    public InnoGotchiComponentDto() : this(null, null)
    {

    }
}