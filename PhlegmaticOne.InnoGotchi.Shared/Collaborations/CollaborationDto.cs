namespace PhlegmaticOne.InnoGotchi.Shared.Collaborations;

public class CollaborationDto
{
    public string CurrentUserEmail { get; set; } = null!;
    public string CollaboratedUserEmail { get; set; } = null!;
    public string FarmName { get; set; } = null!; 
}