namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

public class CreateInnoGotchiViewModel
{
    public List<CreateInnoGotchiComponentViewModel> Components { get; set; } = null!;
}

public class CreateInnoGotchiComponentViewModel
{
    public float TranslateX { get; set; }
    public float TranslateY { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public string ImageUrl { get; set; } = null!;
}