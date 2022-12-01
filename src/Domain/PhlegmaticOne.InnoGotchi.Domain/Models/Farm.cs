using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class Farm : EntityBase
{
    public string Name { get; set; } = null!;
    public Guid OwnerId { get; set; }
    public UserProfile Owner { get; set; } = null!;
    public IList<InnoGotchiModel> InnoGotchies { get; set; } = new List<InnoGotchiModel>();
    public IList<Collaboration> Collaborations { get; set; } = new List<Collaboration>();
    public FarmStatistics FarmStatistics { get; set; } = null!;
}