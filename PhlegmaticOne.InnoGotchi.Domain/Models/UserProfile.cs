using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class UserProfile : EntityBase
{
    public DateTime JoinDate { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Avatar? Avatar { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Farm Farm { get; set; } = null!;
    public IList<Collaboration> Collaborations { get; set; } = new List<Collaboration>();
    public IList<Invitation> SentInvitations { get; set; } = new List<Invitation>();
    public IList<Invitation> ReceivedInvitations { get; set; } = new List<Invitation>();
}