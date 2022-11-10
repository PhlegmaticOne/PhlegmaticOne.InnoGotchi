using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class Farm : EntityBase
{
    public string Name { get; set; }
    public IEnumerable<InnoGotchiModel> InnoGotchies { get; set; }
    public Guid OwnerId { get; set; }
    public UserProfile Owner { get; set; }
    public IEnumerable<Collaboration> Collaborations { get; set; }
}