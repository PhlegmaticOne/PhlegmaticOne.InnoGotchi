namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Charts;

public class BarChartViewModel
{
    public List<BarChartItemViewModel> BarItems { get; init; }
    public List<string> Labels { get; init; }
    public List<string> Colors { get; init; }
}

public class BarChartItemViewModel
{
    public BarChartItemViewModel(string x, double a, double b)
    {
        X = x;
        A = a;
        B = b;
    }

    public string X { get; }
    public double A { get; }
    public double B { get; }
}