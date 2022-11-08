using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class Avatar : ModelBase
{
    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
}