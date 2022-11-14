namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

public class ConstructorViewModel
{
    public List<ComponentCategoryViewModel> ComponentCategories { get; set; } = null!; 
}

public class ComponentCategoryViewModel
{
    public string CategoryName { get; set; } = null!;
    public List<ComponentViewModel> Components { get; set; } = null!;
}

public class ComponentViewModel
{
    public string ImageUrl { get; set; } = null!;
    public string Name { get; set; } = null!;
}