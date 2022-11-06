using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class Farm : ModelBase
{
    public string Name { get; set; }
    public IEnumerable<InnoGotchi> InnoGotchies { get; set; }
    public Guid OwnerId { get; set; }
    public UserProfile Owner { get; set; }
    public IEnumerable<UserProfile> Collaborators { get; set; }
}