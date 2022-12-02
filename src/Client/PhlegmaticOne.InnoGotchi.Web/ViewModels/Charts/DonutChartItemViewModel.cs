namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Charts;

public class DonutChartItemViewModel
{
    public DonutChartItemViewModel(string label, double value, string color)
    {
        Label = label;
        Value = value;
        Color = color;
    }

    public string Label { get; init; }
    public double Value { get; init; }
    public string Color { get; init; }
}