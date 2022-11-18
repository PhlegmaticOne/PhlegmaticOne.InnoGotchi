using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class User : EntityBase
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserProfile Profile { get; set; } = null!;
}