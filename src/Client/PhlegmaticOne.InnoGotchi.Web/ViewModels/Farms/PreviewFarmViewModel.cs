namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Farms;

public class PreviewFarmViewModel
{
    public Guid FarmId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public byte[] OwnerAvatarData { get; set; } = null!;
}