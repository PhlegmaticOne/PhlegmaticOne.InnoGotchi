namespace PhlegmaticOne.InnoGotchi.Shared.Profiles;

public class DetailedProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] AvatarData { get; set; } = null!;
    public DateTime JoinDate { get; set; }
}