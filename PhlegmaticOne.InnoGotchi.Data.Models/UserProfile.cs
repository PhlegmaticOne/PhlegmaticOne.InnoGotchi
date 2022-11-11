using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class UserProfile : EntityBase
{
    public DateTime JoinDate { get; set; }
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public Avatar Avatar { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Farm Farm { get; set; } = null!;
    public IList<Collaboration> Collaborations { get; set; } = null!;
    public IList<Invitation> SentInvitations { get; set; } = null!;
    public IList<Invitation> ReceivedInvitations { get; set; } = null!;
}