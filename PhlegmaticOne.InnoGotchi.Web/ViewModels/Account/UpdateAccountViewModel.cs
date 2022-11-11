using System.ComponentModel.DataAnnotations;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

public class UpdateAccountViewModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public IFormFile? Avatar { get; set; }
    [DataType(DataType.Password)] public string OldPassword { get; set; } = null!;
    [DataType(DataType.Password)] public string NewPassword { get; set; } = null!;
}