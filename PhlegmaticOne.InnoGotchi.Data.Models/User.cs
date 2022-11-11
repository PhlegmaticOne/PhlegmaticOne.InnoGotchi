using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class User : EntityBase
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserProfile Profile { get; set; } = null!;
}