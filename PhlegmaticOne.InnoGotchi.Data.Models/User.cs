using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class User : ModelBase
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid ProfileId { get; set; }
    public UserProfile Profile { get; set; }
}