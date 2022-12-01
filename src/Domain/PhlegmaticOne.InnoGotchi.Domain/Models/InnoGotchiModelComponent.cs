namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class InnoGotchiModelComponent
{
    public Guid InnoGotchiId { get; set; }
    public InnoGotchiModel InnoGotchi { get; set; } = null!;
    public Guid ComponentId { get; set; }
    public InnoGotchiComponent InnoGotchiComponent { get; set; } = null!;
    public float TranslationX { get; set; }
    public float TranslationY { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
}