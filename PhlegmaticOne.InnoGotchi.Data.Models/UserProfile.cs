using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class UserProfile : ModelBase
{
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public byte[] AvatarData { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Farm Farm { get; set; }
    public IEnumerable<Farm> Collaborations { get; set; }
}