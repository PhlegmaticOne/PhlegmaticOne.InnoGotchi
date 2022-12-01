namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;

public class IdentityModel<T>
{
    public Guid ProfileId { get; set; }
    public T Entity { get; init; } = default!;
}