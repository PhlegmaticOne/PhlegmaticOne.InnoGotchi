using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class User : EntityBase
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserProfile Profile { get; set; }
}