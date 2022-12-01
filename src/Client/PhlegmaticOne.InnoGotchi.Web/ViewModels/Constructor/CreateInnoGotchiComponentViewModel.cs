namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

public class CreateInnoGotchiComponentViewModel
{
    public string Name { get; set; } = null!;
    public float TranslationX { get; set; }
    public float TranslationY { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public string ImageUrl { get; set; } = null!;
}