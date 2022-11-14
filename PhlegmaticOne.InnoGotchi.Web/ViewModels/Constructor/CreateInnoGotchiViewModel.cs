namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

public class CreateInnoGotchiViewModel
{
    public string Name { get; set; } = null!;
    public List<CreateInnoGotchiComponentViewModel> Components { get; set; } = null!;
}

public class CreateInnoGotchiComponentViewModel
{
    public float TranslationX { get; set; }
    public float TranslationY { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public string ImageUrl { get; set; } = null!;
}