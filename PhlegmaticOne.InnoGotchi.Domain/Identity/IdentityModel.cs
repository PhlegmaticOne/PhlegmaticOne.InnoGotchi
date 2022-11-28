namespace PhlegmaticOne.InnoGotchi.Domain.Identity;

public class IdentityModel<T> : IHaveProfileId
{
    public Guid ProfileId { get; set; }
    public T Entity { get; init; } = default!;

    public IdentityModel<TNew> Transform<TNew>(TNew nNew)
    {
        return new IdentityModel<TNew>
        {
            Entity = nNew,
            ProfileId = ProfileId
        };
    }
}