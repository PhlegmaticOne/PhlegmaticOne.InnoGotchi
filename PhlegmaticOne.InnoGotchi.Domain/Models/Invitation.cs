using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class Invitation : EntityBase
{
    public Guid FromProfileId { get; set; }
    public UserProfile From { get; set; } = null!;
    public Guid ToProfileId { get; set; }
    public UserProfile To { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public InvitationStatus InvitationStatus { get; set; }
}