using PhlegmaticOne.InnoGotchi.Web.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

public class LoginViewModel : ErrorHavingViewModel
{
    [DataType(DataType.EmailAddress)] public string Email { get; set; } = null!;

    [DataType(DataType.Password)] public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
}