namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class InnoGotchiActionViewModel
{
    public Guid InnoGotchiId { get; set; }
    public string ActionName { get; set; }
    public string ActionText { get; set; }
    public bool IsDisabled { get; set; }
}