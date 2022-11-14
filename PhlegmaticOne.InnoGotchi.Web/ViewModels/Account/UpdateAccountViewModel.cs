﻿using System.ComponentModel.DataAnnotations;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Base;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

public class UpdateAccountViewModel : ErrorHavingViewModel
{
    public Guid Id { get; set; }
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