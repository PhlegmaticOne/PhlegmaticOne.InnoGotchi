using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class Avatar : EntityBase
{
    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
}