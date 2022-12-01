namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class InnoGotchiActionViewModel
{
    public Guid InnoGotchiId { get; set; }
    public string ActionName { get; set; } = null!;
    public string ActionText { get; set; } = null!;
    public bool IsDisabled { get; set; }
}