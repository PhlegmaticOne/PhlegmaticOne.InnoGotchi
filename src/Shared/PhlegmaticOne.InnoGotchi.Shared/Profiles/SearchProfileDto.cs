namespace PhlegmaticOne.InnoGotchi.Shared.Profiles;

public class SearchProfileDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public bool IsAlreadyCollaborated { get; set; }
}