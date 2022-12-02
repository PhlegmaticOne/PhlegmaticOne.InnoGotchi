using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class UserProfile : EntityBase
{
    public DateTime JoinDate { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Guid AvatarId { get; set; }
    public Avatar? Avatar { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Farm? Farm { get; set; }
    public IList<Collaboration> Collaborations { get; set; } = new List<Collaboration>();
}