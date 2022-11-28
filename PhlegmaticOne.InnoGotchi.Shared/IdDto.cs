namespace PhlegmaticOne.InnoGotchi.Shared;

public class IdDto
{
    public Guid Id { get; set; }
    public IdDto(Guid id) => Id = id;
}