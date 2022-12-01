namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

public class InnoGotchiLinkViewModel
{
    public string Description { get; set; } = null!;
    public int LinkSortType { get; set; }
    public int CurrentSortType { get; set; }
    public int PageSize { get; set; }
    public bool IsAscending { get; set; }
}