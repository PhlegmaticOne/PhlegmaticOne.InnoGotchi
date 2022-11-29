namespace PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;

public class InnoGotchiRequestDto
{
    public Guid PetId { get; set; }

    public InnoGotchiRequestDto(Guid petId) { PetId = petId; }
    public InnoGotchiRequestDto() { }
}