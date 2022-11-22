namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Accounts;

public class SearchProfileViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool IsAlreadyCollaborated { get; set; }
}