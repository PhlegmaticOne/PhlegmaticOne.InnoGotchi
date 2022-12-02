using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Models;

public class InnoGotchiComponent : EntityBase
{
    public string ImageUrl { get; set; } = null!;
    public string Name { get; set; } = null!;

    public IList<InnoGotchiModelComponent> InnoGotchiModelComponents { get; set; } =
        new List<InnoGotchiModelComponent>();
}