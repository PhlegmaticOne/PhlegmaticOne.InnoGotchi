﻿using System.ComponentModel.DataAnnotations;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Не указан Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Пароль введен неверно")]
    public string ConfirmPassword { get; set; }
}