namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

public class ConstructorViewModel
{
    public IEnumerable<IGrouping<string, string>> ComponentsByCategories { get; init; } = null!;
}