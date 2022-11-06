namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class InnoGotchiModelComponent
{
    public Guid InnoGotchiId { get; set; }
    public InnoGotchiModel InnoGotchi { get; set; }
    public Guid ComponentId { get; set; }
    public InnoGotchiComponent InnoGotchiComponent { get; set; }
    public float TranslationX { get; set; }
    public float TranslationY { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
}