namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.ViewComponents.Base;

public class InnoGotchiStatusGenerator : IInnoGotchiStatusGenerator
{
    private readonly Dictionary<string, string> _statusesToClasses = new()
    {
        { "dead", "bg-dark text-white" },
        { "normal", "bg-warning text-dark" },
        { "hungry", "bg-danger text-white" },
        { "thirsty", "bg-danger text-white" },
        { "full", "bg-success text-white" },
        { "newborn", "bg-info text-dark" },
        { "alive", "bg-light text-dark" },
    };

    public string GenerateBadgeCssClasses(string petStatus) => 
        _statusesToClasses.TryGetValue(petStatus.ToLower(), out var cssClasses) ?
            cssClasses : 
            _statusesToClasses.Last().Value;
}