using System.ComponentModel.DataAnnotations;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Specify your Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Specify your first name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Specify your second name")]
    public string SecondName { get; set; }

    [Required(ErrorMessage = "Specify your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Two passwords aren't equal")]
    public string ConfirmPassword { get; set; }
}