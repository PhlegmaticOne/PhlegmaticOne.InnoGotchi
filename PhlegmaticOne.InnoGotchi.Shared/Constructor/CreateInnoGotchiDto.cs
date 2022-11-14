namespace PhlegmaticOne.InnoGotchi.Shared.Constructor;

public class CreateInnoGotchiDto
{
    public string Name { get; set; } = null!;
    public List<CreateInnoGotchiComponentDto> Components { get; set; } = null!;
}