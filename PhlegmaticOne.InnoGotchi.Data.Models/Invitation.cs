using PhlegmaticOne.InnoGotchi.Data.Models.Base;
using PhlegmaticOne.InnoGotchi.Data.Models.Enums;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class Invitation : ModelBase
{
    public Guid FromProfileId { get; set; }
    public UserProfile From { get; set; }
    public Guid ToProfileId { get; set; }
    public UserProfile To { get; set; }
    public DateTime SentAt { get; set; }
    public InvitationStatus InvitationStatus { get; set; }
}