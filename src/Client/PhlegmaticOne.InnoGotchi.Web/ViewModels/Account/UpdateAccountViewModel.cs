using PhlegmaticOne.InnoGotchi.Web.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

public class UpdateAccountViewModel : ErrorHavingViewModel
{
    public string OldFirstName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string OldLastName { get; set; } = null!;
    public string? LastName { get; set; }
    public string? CurrentAvatar { get; set; }
    public IFormFile? Avatar { get; set; }
    [DataType(DataType.Password)] public string? OldPassword { get; set; }
    [DataType(DataType.Password)] public string? NewPassword { get; set; }
    [DataType(DataType.Password)] public string? NewPasswordConfirm { get; set; }
}