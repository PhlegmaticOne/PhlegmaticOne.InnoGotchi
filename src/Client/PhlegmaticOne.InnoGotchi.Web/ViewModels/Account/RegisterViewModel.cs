using PhlegmaticOne.InnoGotchi.Web.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

public class RegisterViewModel : ErrorHavingViewModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public IFormFile? Avatar { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; } = null!;

    [DataType(DataType.Password)] public string Password { get; set; } = null!;

    [DataType(DataType.Password)] public string ConfirmPassword { get; set; } = null!;
}