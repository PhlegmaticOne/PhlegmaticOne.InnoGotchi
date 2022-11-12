using System.ComponentModel.DataAnnotations;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

public class UpdateAccountViewModel
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public byte[]? CurrentAvatar { get; set; }
    public IFormFile? Avatar { get; set; }
    [DataType(DataType.Password)] public string? OldPassword { get; set; }
    [DataType(DataType.Password)] public string? NewPassword { get; set; }
    [DataType(DataType.Password)] public string? NewPasswordConfirm { get; set; }
}