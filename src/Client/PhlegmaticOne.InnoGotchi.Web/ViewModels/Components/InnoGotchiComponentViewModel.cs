namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Components;

public class InnoGotchiComponentViewModel
{
    public float TranslationX { get; set; }
    public float TranslationY { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string Name { get; set; } = null!;
}