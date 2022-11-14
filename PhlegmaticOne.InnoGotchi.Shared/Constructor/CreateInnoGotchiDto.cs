namespace PhlegmaticOne.InnoGotchi.Shared.Constructor;

public class CreateInnoGotchiDto
{
    public string Name { get; set; } = null!;
    public List<InnoGotchiComponentDto> Components { get; set; } = null!;
}