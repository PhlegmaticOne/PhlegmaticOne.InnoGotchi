namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class Collaboration
{
    public Guid UserProfileId { get; set; }
    public UserProfile Collaborator { get; set; }
    public Guid FarmId { get; set; }
    public Farm Farm { get; set; }
}