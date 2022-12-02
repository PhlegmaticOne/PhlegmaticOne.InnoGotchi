namespace PhlegmaticOne.InnoGotchi.Shared.Collaborations;

public class CreateCollaborationDto
{
    public CreateCollaborationDto(Guid toProfileId)
    {
        ToProfileId = toProfileId;
    }

    public Guid ToProfileId { get; }
}