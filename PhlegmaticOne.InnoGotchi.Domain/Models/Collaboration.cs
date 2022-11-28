using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class Collaboration : EntityBase
{
    public Guid UserProfileId { get; set; }
    public UserProfile Collaborator { get; set; } = null!;
    public Guid FarmId { get; set; }
    public Farm Farm { get; set; } = null!;
}