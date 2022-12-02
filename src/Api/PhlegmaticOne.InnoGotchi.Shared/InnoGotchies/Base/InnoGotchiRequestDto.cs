namespace PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;

public class InnoGotchiRequestDto
{
    public InnoGotchiRequestDto(Guid petId)
    {
        PetId = petId;
    }

    public InnoGotchiRequestDto()
    {
    }

    public Guid PetId { get; set; }
}