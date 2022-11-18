namespace PhlegmaticOne.InnoGotchi.Domain.Identity;

public class IdentityModel<T>
{
    public Guid ProfileId { get; init; }
    public T Entity { get; init; }
}