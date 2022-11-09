using System.ComponentModel.DataAnnotations;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Specify the email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Specify the password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }
}