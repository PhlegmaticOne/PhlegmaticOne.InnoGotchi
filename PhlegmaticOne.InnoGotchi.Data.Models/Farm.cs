using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class Farm : EntityBase
{
    public string Name { get; set; } = null!;
    public IList<InnoGotchiModel> InnoGotchies { get; set; } = null!;
    public Guid OwnerId { get; set; }
    public UserProfile Owner { get; set; } = null!;
    public IList<Collaboration> Collaborations { get; set; } = null!;
}